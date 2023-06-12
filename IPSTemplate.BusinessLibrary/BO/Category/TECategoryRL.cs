using Core.DAL.Interface;
using Core.Library.Base;
using Csla;
using IPSTemplate.Dal.Models;

namespace IPSTemplate.BusinessLibrary.BO.Category
{
    [Serializable]
    public class TECategoryRL : CslaReadOnlyListBase<TECategoryRL, TECategoryInfo, TECategory>
    {
        public TECategoryRL()
        { }

        #region Client-side methods
        public static async Task<TECategoryRL> GetFilteredList(string? filter, IDataPortalFactory dataPortalFactory)
        {
            return await dataPortalFactory.GetPortal<TECategoryRL>().FetchAsync(filter);
        }
        #endregion

        #region Server-side methods
        [Fetch]
        protected async Task FetchFilteredList(string? filter, [Inject] IRepository<TECategory, TECategory> repository, [Inject] IChildDataPortalFactory childFactory)
        {
            if (!string.IsNullOrEmpty(filter))
                Fetch(p => p.Name.Contains(filter), repository, childFactory);
            else
                Fetch(repository, childFactory);
        }
        #endregion
    }
}
