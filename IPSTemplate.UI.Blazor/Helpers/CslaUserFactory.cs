using System.Security.Claims;
using Core.Library.BO.User;
using Core.Library.Extensions;
using Core.Library.Security;
using Csla;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

namespace IPSTemplate.UI.Blazor.Helpers
{
    public class CslaUserFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        private readonly ApplicationContext _appContext;
        private readonly IDataPortalFactory _dataPortalFactory;

        public CslaUserFactory(IAccessTokenProviderAccessor accessor, ApplicationContext appContext, IDataPortalFactory dataPortalFactory)
            : base(accessor)
        {
            _appContext = appContext;
            _dataPortalFactory = dataPortalFactory;
        }

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
        {
            var user = await base.CreateUserAsync(account, options);
            if (user.Identity?.IsAuthenticated == true)
            {
                try
                {
                    var identity = (ClaimsIdentity)user.Identity;
                    if (!Principal.HasCslaIdentity(_appContext))
                    {
                        Guid? userID = null;
                        var userIdClaim = user.Claims.FirstOrDefault(p => p.Type == OktaClaimTypes.UserId);
                        if (!userIdClaim.IsNull() && !userIdClaim.Value.IsNull())
                            userID = Guid.TryParse(userIdClaim.Value, out Guid userId) ? userId : new Guid();

                        if (userID.HasValue && userID != Guid.Empty)
                        {
                            var userInfo = await SYUserLoginInfo.GetItemAsync(userID, _dataPortalFactory);
                            var cslaIdentity = userInfo.GetIdentity();

                            foreach (var role in user.Claims.Where(p => p.Type == OktaClaimTypes.Groups))
                                cslaIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Value.Trim()));

                            user.AddIdentity(cslaIdentity);
                        }
                    }
                    _appContext.User = user;
                }
                catch (AccessTokenNotAvailableException e)
                {
                    e.Redirect();
                }
            }

            return user;
        }
    }
}
