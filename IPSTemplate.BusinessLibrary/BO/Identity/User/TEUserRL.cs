using Core.DAL.Infrastructure;
using Core.Library.Base;
using Csla;

namespace IPSTemplate.BusinessLibrary.BO.Identity.User
{
    [Serializable]
    public class TEUserRL : CslaReadOnlyListBase<TEUserRL, TEUserInfo, Entity>
    {
        public TEUserRL()
        {
        }

        #region Client-side methods

        public void EnableEditing(bool enable)
        {
            IsReadOnly = !enable;
        }

        //public static async Task<TEUserRL> GetFilteredList(string? filter, IDataPortalFactory dataPortalFactory)
        //{
        //    return await dataPortalFactory.GetPortal<TEUserRL>().FetchAsync(filter);
        //}

        public static new async Task<TEUserRL> GetListAsync(IDataPortalFactory factory)
        {
            return await factory.GetPortal<TEUserRL>().FetchAsync(true);
        }

        public static new async Task<TEUserRL> GetListAsync(MobileCslaRequest req, IDataPortalFactory factory, bool loadRoles = true)
        {
            return await factory.GetPortal<TEUserRL>().FetchAsync(req, loadRoles);
        }

        #endregion

        #region Server-side methods

        [Fetch]
        protected async Task Fetch(MobileCslaRequest req, bool loadRoles, [Inject] IDataPortalFactory factory)
        {
            var users = await TEUserGetListCommand.GetUserListAsync(req, factory, loadRoles);
            this.EnableEditing(true);
            using (SuppressListChangedEvents)
            {
                this.AddRange(users.Data);
            }
            this.EnableEditing(false);
        }

        [Fetch]
        protected async Task Fetch(bool x, [Inject] IDataPortalFactory factory)
        {
            var users = await TEUserGetListCommand.GetUserListAsync(null, factory);
            this.EnableEditing(true);
            using (SuppressListChangedEvents)
            {
                this.AddRange(users.Data);
            }
            this.EnableEditing(false);
        }

        #endregion
    }
}
