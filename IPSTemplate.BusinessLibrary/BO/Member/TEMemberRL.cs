using Core.DAL.Interface;
using Core.Library.Base;
using Csla;
using Csla.Core;
using IPSTemplate.BusinessLibrary.BO.Member;
using IPSTemplate.Dal.Models;

[Serializable]
public class TEMemberRL : CslaReadOnlyListBase<TEMemberRL, TEMemberInfo, TEMember>
{
    public TEMemberRL()
    { }

    #region Client-side methods
    public static async Task<TEMemberRL> GetFilteredList(string? filter, IDataPortalFactory dataPortalFactory)
    {
        return await dataPortalFactory.GetPortal<TEMemberRL>().FetchAsync(filter);
    }

    public static TEMemberRL GetListByIds(IEnumerable<Guid> ids, IDataPortalFactory dataPortalFactory)
    {
        return dataPortalFactory.GetPortal<TEMemberRL>().Fetch(new MobileList<Guid>(ids));
    }

    #endregion


    #region Server-side methods
    [Fetch]
    protected async Task FetchFilteredList(string? filter, [Inject] IRepository<TEMember, TEMember> repository, [Inject] IChildDataPortalFactory childFactory)
    {
        if (!string.IsNullOrEmpty(filter))
            Fetch(p => p.FirstName.Contains(filter) || p.LastName.Contains(filter), repository, childFactory);
        else
            Fetch(repository, childFactory);
    }

    [Fetch]
    protected void GetListByIds(MobileList<Guid> ids, [Inject] IRepository<TEMember, TEMember> repository, [Inject] IChildDataPortalFactory childFactory)
    {
        Fetch(p => ids.Contains(p.Id), repository, childFactory);
    }

    #endregion

}
