using Core.DAL.Interface;
using Core.Library.Base;
using Csla;
using IPSTemplate.Dal.Models;

namespace IPSTemplate.BusinessLibrary.BO.Book
{
    [Serializable]
    public class TEBookAuthorEL : CslaBusinessListBase<TEBookAuthorEL, TEBookAuthorBO, TEBookAuthor>
    {
        public TEBookAuthorEL()
        { }

        #region Client-side methods
        public static TEBookAuthorEL GetByBookId(Guid bookId, IDataPortalFactory factory)
        {
            return factory.GetPortal<TEBookAuthorEL>().Fetch(bookId, false);
        }
        #endregion

        #region Server-side methods

        [Fetch]
        protected void FetchByBookId(Guid id, bool _, [Inject] IRepository<TEBookAuthor, TEBookAuthor> repository, [Inject] IChildDataPortalFactory childFactory)
        {
            Fetch(p => p.BookID == id, repository, childFactory);
        }

        #endregion



    }
}
