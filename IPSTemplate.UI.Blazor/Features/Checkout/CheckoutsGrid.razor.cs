using Core.Library.Base;
using IPSBlazor.Components;
using IPSBlazor.Services;
using IPSTemplate.BusinessLibrary.BO.Checkout;
using IPSTemplate.Dal.Models;
using IPSTemplate.Dal.Models.Identity;
using IPSTemplate.UI.Blazor.Base;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Telerik.Blazor.Components;
using Telerik.DataSource;

namespace IPSTemplate.UI.Blazor.Features.Checkout;

public partial class CheckoutsGrid
    : GridView<CheckoutsGrid,
               TECheckoutInfo,
               TECheckoutGridInfo,
               TECheckoutRL,
               TECheckout>

{
    private IPSGrid<TECheckoutInfo> _ref = default!;
    private ClaimsPrincipal user = default!;
    public Guid? UserId = default!;

    [Parameter] public EventCallback<TECheckoutInfo> BookReturned { get; set; }

    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    [Inject] public UserManager<TEIdentityUser> UserManager { get; set; } = default!;
    [Inject] NotificationService NotificationService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        user = auth.User;
        var nekaj = user.Identity.IsAuthenticated;
        var claims = user.Claims.ToList();
        var role = claims[1].Value;
        if (!(role == "Administrator"))//ce ni admin mu daj userId
        {
            UserId = Guid.Parse(UserManager.GetUserId(user));
        }
        await base.OnInitializedAsync();
    }

    protected override async Task<TECheckoutGridInfo> GetGridData(MobileCslaRequest request)
    {
        return await TECheckoutGridInfo.GetGridInfoAsync(request, UserId, DataPortalFactory);
    }


    public void bookReturnedNotification(TECheckoutInfo context)
    {
        NotificationService.ShowInfo("Knjiga je uspešno vrnjena.");
    }

    private async Task OnStateInitHandler(GridStateEventArgs<TECheckoutInfo> args)
    {
        var state = new GridState<TECheckoutInfo>
        {
            SortDescriptors = new List<SortDescriptor>
                    {
                        new SortDescriptor{ Member="IsReturned", SortDirection = ListSortDirection.Ascending  }
                    },
            FilterDescriptors = new List<IFilterDescriptor>()
        };
        args.GridState = state;
    }


    public void Rebind() => _ref.Rebind();
}

