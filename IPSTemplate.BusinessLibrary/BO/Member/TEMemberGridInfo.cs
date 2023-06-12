using Core.Library.Base;
using Csla;
using IPSTemplate.Dal.Models;

namespace IPSTemplate.BusinessLibrary.BO.Member
{
    [Serializable]
    public class TEMemberGridInfo : CslaReadOnlyGridBase<TEMemberGridInfo, TEMemberRL, TEMember, TEMemberRL, TEMemberInfo>
    {
        #region Client-side methods

        public static async Task<TEMemberGridInfo> GetGridInfoAsync(MobileCslaRequest request, IDataPortalFactory factory)
        {
            return await factory.GetPortal<TEMemberGridInfo>().FetchAsync(request);
        }

        #endregion

        #region Server-side methods

        [Fetch]
        protected async Task FetchGridInfo(MobileCslaRequest request, [Inject] IDataPortalFactory factory)
        {
            Data = await TEMemberRL.GetListAsync(request, factory);
            TotalRowCount = Math.Max(0, Data.TotalRowCount);
        }

        #endregion
    }
}
