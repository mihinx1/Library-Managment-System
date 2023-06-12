using Core.DAL.Interface;
using Core.Library.Base;
using Csla;
using Csla.Core;
using IPSTemplate.BusinessLibrary.BO.Author;
using IPSTemplate.Dal.Models;

[Serializable]
public class TEAuthorRL : CslaReadOnlyListBase<TEAuthorRL, TEAuthorInfo, TEAuthor>
{
    public TEAuthorRL()
    { }

    #region Client-side methods
    public static async Task<TEAuthorRL> GetFilteredList(string? filter, IDataPortalFactory dataPortalFactory)
    {
        return await dataPortalFactory.GetPortal<TEAuthorRL>().FetchAsync(filter);
    }

    public static TEAuthorRL GetListByIds(IEnumerable<Guid> ids, IDataPortalFactory dataPortalFactory)
    {
        return dataPortalFactory.GetPortal<TEAuthorRL>().Fetch(new MobileList<Guid>(ids));
    }

    #endregion


    #region Server-side methods
    [Fetch]
    protected async Task FetchFilteredList(string? filter, [Inject] IRepository<TEAuthor, TEAuthor> repository, [Inject] IChildDataPortalFactory childFactory)
    {
        if (!string.IsNullOrEmpty(filter))
            Fetch(p => p.FirstName.Contains(filter) || p.LastName.Contains(filter), repository, childFactory);
        else
            Fetch(repository, childFactory);
    }

    [Fetch]
    protected void GetListByIds(MobileList<Guid> ids, [Inject] IRepository<TEAuthor, TEAuthor> repository, [Inject] IChildDataPortalFactory childFactory)
    {
        Fetch(p => ids.Contains(p.Id), repository, childFactory);
    }

    #endregion

}
