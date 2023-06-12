using Core.DAL.Interface;
using Core.Library.Base;
using Csla;
using IPSTemplate.Dal.Models;

namespace IPSTemplate.BusinessLibrary.BO.Book
{
    [Serializable]
    public class TEBookRL : CslaReadOnlyListBase<TEBookRL, TEBookInfo, TEBook>
    {
        public TEBookRL()
        { }

        #region Client-side methods
        public static async Task<TEBookRL> GetFilteredList(string? filter, IDataPortalFactory dataPortalFactory)
        {
            return await dataPortalFactory.GetPortal<TEBookRL>().FetchAsync(filter);
        }

        #endregion

        #region Server-side methods
        [Fetch]
        protected async Task FetchFilteredList(string? filter, [Inject] IRepository<TEBook, TEBook> repository, [Inject] IChildDataPortalFactory childFactory)
        {
            if (!string.IsNullOrEmpty(filter))
                Fetch(p => p.Title.Contains(filter) || p.Language.Contains(filter), repository, childFactory);
            else
                Fetch(repository, childFactory);
        }

        #endregion
    }
}
