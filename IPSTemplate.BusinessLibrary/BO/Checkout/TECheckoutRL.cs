using Core.Library.Base;
using IPSTemplate.BusinessLibrary.BO.Checkout;
using IPSTemplate.Dal.Models;

[Serializable]
public class TECheckoutRL : CslaReadOnlyListBase<TECheckoutRL, TECheckoutInfo, TECheckout>
{
    public TECheckoutRL()
    { }

    #region Client-side methods
    //public static async Task<TECheckoutRL> GetFilteredList(string? filter, IDataPortalFactory dataPortalFactory)
    //{
    //    return await dataPortalFactory.GetPortal<TECheckoutRL>().FetchAsync(filter);
    //}

    //public static TECheckoutRL GetListByIds(IEnumerable<Guid> ids, IDataPortalFactory dataPortalFactory)
    //{
    //    return dataPortalFactory.GetPortal<TECheckoutRL>().Fetch(new MobileList<Guid>(ids));
    //}

    #endregion


    #region Server-side methods

    //[Fetch]
    //protected async Task FetchFilteredList(string? filter, [Inject] IRepository<TECheckout, TECheckout> repository, [Inject] IChildDataPortalFactory childFactory)
    //{
    //    if (!string.IsNullOrEmpty(filter))
    //        Fetch(p => p.MemberID.Contains(filter));
    //    else
    //        Fetch(repository, childFactory);
    //}

    //[Fetch]
    //protected void GetListByIds(MobileList<Guid> ids, [Inject] IRepository<TECheckout, TECheckout> repository, [Inject] IChildDataPortalFactory childFactory)
    //{
    //    Fetch(p => ids.Contains(p.Id), repository, childFactory);
    //}

    #endregion

}
