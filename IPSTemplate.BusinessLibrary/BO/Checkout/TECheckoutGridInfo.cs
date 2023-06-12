using Core.Library.Base;
using Csla;
using Csla.Core;
using IPSTemplate.Dal.Models;

namespace IPSTemplate.BusinessLibrary.BO.Checkout
{
    [Serializable]
    public class TECheckoutGridInfo : CslaReadOnlyGridBase<TECheckoutGridInfo, TECheckoutRL, TECheckout, TECheckoutRL, TECheckoutInfo>
    {
        #region Client-side methods

        public static async Task<TECheckoutGridInfo> GetGridInfoAsync(MobileCslaRequest request, IDataPortalFactory factory)
        {
            return await factory.GetPortal<TECheckoutGridInfo>().FetchAsync(request);
        }

        public static async Task<TECheckoutGridInfo> GetGridInfoAsync(MobileCslaRequest request, Guid? userId, IDataPortalFactory factory)
        {
            if (userId.HasValue)
            {
                request.Filters.Add(CslaRequestFilter.CreateNewFilter(nameof(TECheckoutInfo.UserID), typeof(Guid), 2, userId, factory.GetPortal<CslaRequestFilter>()));
            }
            return await factory.GetPortal<TECheckoutGridInfo>().FetchAsync(request);
        }

        #endregion

        #region Server-side methods

        [Fetch]
        protected async Task FetchGridInfo(MobileCslaRequest request, [Inject] IDataPortalFactory factory)
        {
            //request.Include.Add(nameof(TECheckout.Member));
            request.Include = new MobileList<string>() { "BookCopy", "BookCopy.Book", "User" };
            Data = await TECheckoutRL.GetListAsync(request, factory);
            TotalRowCount = Math.Max(0, Data.TotalRowCount);
        }

        #endregion
    }
}
