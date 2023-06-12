using Core.DAL.Interface;
using Core.Library.Base;
using Csla;
using IPSTemplate.Dal.Models;

namespace IPSTemplate.BusinessLibrary.BO.Publisher
{
    [Serializable]
    public class TEPublisherRL : CslaReadOnlyListBase<TEPublisherRL, TEPublisherInfo, TEPublisher>
    {
        public TEPublisherRL()
        { }

        #region Client-side methods
        public static async Task<TEPublisherRL> GetFilteredList(string? filter, IDataPortalFactory dataPortalFactory)
        {
            return await dataPortalFactory.GetPortal<TEPublisherRL>().FetchAsync(filter);
        }
        #endregion

        #region Server-side methods
        [Fetch]
        protected async Task FetchFilteredList(string? filter, [Inject] IRepository<TEPublisher, TEPublisher> repository, [Inject] IChildDataPortalFactory childFactory)
        {
            if (!string.IsNullOrEmpty(filter))
                Fetch(p => p.Name.Contains(filter) || p.Location.Contains(filter) || p.Email.Contains(filter), repository, childFactory);
            else
                Fetch(repository, childFactory);
        }
        #endregion
    }
}
