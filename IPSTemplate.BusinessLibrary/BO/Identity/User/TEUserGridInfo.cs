using Core.DAL.Infrastructure;
using Core.Library.Base;
using Csla;

namespace IPSTemplate.BusinessLibrary.BO.Identity.User
{
    [Serializable]
    public class TEUserGridInfo : CslaReadOnlyGridBase<TEUserGridInfo, TEUserRL, Entity, TEUserRL, TEUserInfo>
    {
        public TEUserGridInfo()
        {
        }

        #region Client-side methods
        public static async Task<TEUserGridInfo> GetGridInfoAsync(MobileCslaRequest request, IDataPortalFactory factory)
        {
            return await factory.GetPortal<TEUserGridInfo>().FetchAsync(request);
        }

        #endregion

        #region Server-side methods

        [Fetch]
        protected async Task FetchGridInfo(MobileCslaRequest request, [Inject] IDataPortalFactory factory)
        {
            var usersData = await TEUserGetListCommand.GetUserListAsync(request, factory);
            Data = usersData.Data;
            TotalRowCount = Math.Max(0, usersData.TotalCount);
        }

        #endregion
    }
}
