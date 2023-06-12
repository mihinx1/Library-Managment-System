using Core.Library.Base;
using Csla;
using Csla.Core;
using IPSTemplate.Dal.Models;

namespace IPSTemplate.BusinessLibrary.BO.Book
{
    [Serializable]
    public class TEBookGridInfo : CslaReadOnlyGridBase<TEBookGridInfo, TEBookRL, TEBook, TEBookRL, TEBookInfo>
    {
        #region Client-side methods

        public static async Task<TEBookGridInfo> GetGridInfoAsync(MobileCslaRequest request, IDataPortalFactory factory)
        {
            return await factory.GetPortal<TEBookGridInfo>().FetchAsync(request);
        }

        #endregion

        #region Server-side methods

        [Fetch]
        protected async Task FetchGridInfo(MobileCslaRequest request, [Inject] IDataPortalFactory factory)
        {
            request.Include = new MobileList<string>() { "Category" };
            Data = await TEBookRL.GetListAsync(request, factory);
            TotalRowCount = Math.Max(0, Data.TotalRowCount);
        }

        #endregion
    }
}
