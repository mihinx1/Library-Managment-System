using IPSBlazor.Components;
using IPSBlazor.Extensions;
using IPSTemplate.BusinessLibrary.BO.BookCopy;
using IPSTemplate.BusinessLibrary.BO.Checkout;
using IPSTemplate.BusinessLibrary.BO.Identity.User;
using IPSTemplate.BusinessLibrary.Interfaces;
using IPSTemplate.Dal.Models;
using IPSTemplate.Dal.Models.Identity;
using IPSTemplate.UI.Blazor.Base;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Telerik.Blazor.Components;
namespace IPSTemplate.UI.Blazor.Features.Checkout
{
    public partial class CheckoutEdit : EditView<TECheckoutBO, TECheckout>
    {
        [Inject] protected IDataPortalFactory DataPortalFactory { get; set; } = default!;
        [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject] public UserManager<TEIdentityUser> UserManager { get; set; } = default!;
        [Parameter] public EventCallback ItemSaved { get; set; }
        private ClaimsPrincipal user = default!;
        public Guid UserId = default!;
        public IUserService IUserService = default!;
        protected IPSComboBox<Guid, TEUserInfo> cbxUser = default!;


        //protected async Task GetUsers(ReadEventArgs args)
        //{
        //    var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        //    var user = auth.User;
        //    var curUser = await UserManager.GetUserAsync(user);
        //    var userEmail = curUser.Email;
        //    var claims = user.Claims.ToList();
        //    var role = claims[1].Value;

        //    var request = await DataPortalFactory.GetPortal<MobileCslaRequest>().CreateAsync();
        //    var users = await TEUserRL.GetListAsync(request, DataPortalFactory);
        //    //string? filter = args.Request.GetSingleFilter();
        //    //var customers = await TECustomerRL.GetFilteredList(filter ?? "", DataPortalFactory);

        //    if (role == "Administrator")
        //        args.Data = users;
        //    else
        //        args.Data = users.Where(w => w.Email == userEmail);
        //    cbxUser.Value = users.Where(w => w.Email == userEmail).First().Id;

        //    args.Total = users.Count;
        //}


        protected IPSComboBox<Guid, TEBookCopyInfo>? cbxBook = default!;
        protected async Task GetBookCopies(ReadEventArgs args)

        {

            string? filter = args.Request.GetSingleFilter();
            var books = await TEBookCopyRL.GetFilteredList(filter ?? "", DataPortalFactory);

            args.Data = books;
            args.Total = books.Count;
        }
        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            user = auth.User;
            UserId = Guid.Parse(UserManager.GetUserId(user));
            await base.OnInitializedAsync();
            //UserManager.GetRolesAsync(() => IUserService.GetUsersRoles(TEIdentityUser user));
            ViewModel.Model.UserID = UserId;

            //idi po listi
            //IUserSerivce.cs
            //Task<List<string>> GetUsersRoles(Guid userId);
            //Task<List<string>> GetUsersRoles(TEIdentityUser user);

        }
    }
}
