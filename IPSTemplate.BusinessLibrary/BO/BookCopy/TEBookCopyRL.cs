using Core.DAL.Infrastructure;
using Core.DAL.Interface;
using Core.Library.Base;
using Csla;
using Csla.Core;
using IPSTemplate.Dal.Models;

namespace IPSTemplate.BusinessLibrary.BO.BookCopy
{
    [Serializable]
    public class TEBookCopyRL : CslaReadOnlyListBase<TEBookCopyRL, TEBookCopyInfo, TEBookCopy>
    {
        public TEBookCopyRL()
        { }

        #region Client-side methods
        public static async Task<TEBookCopyRL> GetFilteredList(string? filter, IDataPortalFactory dataPortalFactory)
        {
            return await dataPortalFactory.GetPortal<TEBookCopyRL>().FetchAsync(filter);
        }

        public static TEBookCopyRL GetListByIds(IEnumerable<Guid> ids, IDataPortalFactory dataPortalFactory)
        {
            return dataPortalFactory.GetPortal<TEBookCopyRL>().Fetch(new MobileList<Guid>(ids));
        }

        public static TEBookCopyRL GetByBookId(Guid bookId, IDataPortalFactory factory)
        {
            return factory.GetPortal<TEBookCopyRL>().Fetch(bookId, false);
        }

        #endregion

        #region Server-side methods

        [Fetch]
        protected async Task FetchFilteredList(string? filter, [Inject] IRepository<TEBookCopy, TEBookCopy> repository, [Inject] IDataPortalFactory factory, [Inject] IChildDataPortalFactory childFactory)
        {
            CslaRequest request = new()
            {
                Include = new[] { nameof(TEBookCopy.Book) },
                Predicate = PredicateBuilder
                    .True<TEBookCopy>()
                    .And(p => string.IsNullOrEmpty(filter) || p.Book.Title.Contains(filter))
                    .And(p => p.Checkouts.All(c => c.IsReturned))
                //.And(p => p.IsAvailable)
            };

            Fetch(request, repository, factory, childFactory);
        }

        [Fetch]
        protected void FetchByBookId(Guid id, bool _, [Inject] IRepository<TEBookCopy, TEBookCopy> repository, [Inject] IDataPortalFactory factory, [Inject] IChildDataPortalFactory childFactory)
        {
            var request = new CslaRequest
            {
                Include = new string[] { "Book", "Publisher", "Checkouts" },
                Predicate = PredicateBuilder.True<TEBookCopy>().And(p => p.BookID == id)
            };

            Fetch(request, repository, factory, childFactory);
        }


        [Fetch]
        protected void GetListByIds(MobileList<Guid> ids, [Inject] IRepository<TEBookCopy, TEBookCopy> repository, [Inject] IChildDataPortalFactory childFactory)
        {
            Fetch(p => ids.Contains(p.Id), repository, childFactory);
        }

        #endregion
    }
}
