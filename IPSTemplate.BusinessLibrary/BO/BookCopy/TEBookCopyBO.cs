using Core.Library.Base;
using Csla;
using Csla.Rules;
using IPSTemplate.Dal.Models;
using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.BusinessLibrary.BO.BookCopy
{
    [Serializable]
    public class TEBookCopyBO : CslaBusinessBase<TEBookCopyBO, TEBookCopy>
    {
        #region Properties

        //public static readonly PropertyInfo<DateTime> CreationDateProperty = RegisterProperty<DateTime>(p => p.CreationDate);
        //[Display(Name = "Datum Izdelave")]
        //public DateTime CreationDate
        //{
        //    get => GetProperty(CreationDateProperty);
        //    set => SetProperty(CreationDateProperty, value);
        //}

        public static readonly PropertyInfo<int> PublishedYearProperty = RegisterProperty<int>(p => p.PublishedYear);
        [Display(Name = "Leto izdaje")]
        public int PublishedYear
        {
            get => GetProperty(PublishedYearProperty);
            set => SetProperty(PublishedYearProperty, value);
        }

        public static readonly PropertyInfo<Guid> BookIDProperty = RegisterProperty<Guid>(p => p.BookID);
        [Display(Name = "Id Knjige")]
        public Guid BookID
        {
            get => GetProperty(BookIDProperty);
            set => SetProperty(BookIDProperty, value);
        }

        public static readonly PropertyInfo<string> BookTitleProperty = RegisterProperty<string>(p => p.BookTitle);
        [Display(Name = "Ime Knjige")]
        public string BookTitle
        {
            get => GetProperty(BookTitleProperty);
            set => SetProperty(BookTitleProperty, value);
        }

        public static readonly PropertyInfo<Guid> PublisherIDProperty = RegisterProperty<Guid>(p => p.PublisherID);
        [Display(Name = "Id Založbe")]
        public Guid PublisherID
        {
            get => GetProperty(PublisherIDProperty);
            set => SetProperty(PublisherIDProperty, value);
        }

        public static readonly PropertyInfo<bool> IsAvailableProperty = RegisterProperty<bool>(p => p.IsAvailable);
        [Display(Name = "IsAvailable")]
        public bool IsAvailable
        {
            get => GetProperty(IsAvailableProperty);
            set => SetProperty(IsAvailableProperty, value);
        }


        //public static readonly PropertyInfo<string> PublisherNameProperty = RegisterProperty<string>(p => p.PublisherName);
        //[Display(Name = "Ime Založbe")]
        //public string PublisherName
        //{
        //    get => GetProperty(PublisherNameProperty);
        //    set => SetProperty(PublisherNameProperty, value);
        //}


        //public static readonly PropertyInfo<Guid> BookIDProperty = RegisterProperty<Guid>(p => p.BookID);
        //[Required]
        //public Guid BookID
        //{
        //    get => GetProperty(BookIDProperty);
        //    set => SetProperty(BookIDProperty, value);
        //}

        //public static readonly PropertyInfo<Guid> PublisherIDProperty = RegisterProperty<Guid>(p => p.PublisherID);
        //[Required]
        //public Guid PublisherID
        //{
        //    get => GetProperty(PublisherIDProperty);
        //    set => SetProperty(PublisherIDProperty, value);
        //}

        //public static readonly PropertyInfo<TEBookInfo> BookProperty = RegisterProperty<TEBookInfo>(p => p.Book, RelationshipTypes.LazyLoad);
        //public TEBookInfo Book
        //{
        //    get => LazyGetPropertyAsync(BookProperty, FieldManager.FieldExists(BookProperty) ? Task.FromResult(default(TEBookInfo)) : TEBookInfo.GetItemAsync(BookID, ApplicationContext.GetRequiredService<IDataPortalFactory>()));
        //    set => SetProperty(BookProperty, value);
        //}

        //public static readonly PropertyInfo<TEPublisherInfo> PublisherProperty = RegisterProperty<TEPublisherInfo>(p => p.Book, RelationshipTypes.LazyLoad);
        //public TEPublisherInfo Publisher
        //{
        //    get => LazyGetPropertyAsync(PublisherProperty, FieldManager.FieldExists(PublisherProperty) ? Task.FromResult(default(TEPublisherInfo)) : TEPublisherInfo.GetItemAsync(PublisherID, ApplicationContext.GetRequiredService<IDataPortalFactory>()));
        //    set => SetProperty(PublisherProperty, value);
        //}











        //public static readonly PropertyInfo<TEBookRL> BooksProperty = RegisterProperty<TEBookRL>(p => p.Books, RelationshipTypes.LazyLoad);
        //public TEBookRL Books
        //{
        //    get => LazyGetProperty(BooksProperty, () => GetBooks(ApplicationContext.GetRequiredService<IDataPortalFactory>()));
        //    set => SetProperty(BooksProperty, value);
        //}
        //protected TEBookRL GetBooks(IDataPortalFactory factory)
        //{
        //    return TEBookRL.GetByBookId(Id, factory);
        //}

        //[Display(Name = "Avtorji")]
        //public List<Guid> AuthorIds
        //{
        //    get { return Authors.Any() ? Authors.Select(p => p.AuthorID).ToList() : new List<Guid>(); }
        //    set
        //    {
        //        foreach (var authorId in value.Where(p => !Authors.Select(o => o.AuthorID).ToList().Contains(p)).ToList())
        //        {
        //            var author = ApplicationContext.GetRequiredService<IDataPortalFactory>().GetPortal<TEBookAuthorBO>().Create();
        //            author.BookID = Id;
        //            author.AuthorID = authorId;
        //            Authors.Add(author);
        //        }
        //        foreach (var item in Authors.Where(p => !value.Contains(p.AuthorID)).ToList()) { Authors.Remove(item); }
        //    }
        //}


        #endregion

        #region Validation rules

        public async Task CheckRulesAsync() => await BusinessRules.CheckRulesAsync();
        protected override void AddBusinessRules()
        {
            BusinessRules.AddRule(new OldestPublishedBookRule(PublishedYearProperty));
            base.AddBusinessRules();

        }
        private class OldestPublishedBookRule : BusinessRule
        {
            public OldestPublishedBookRule(Csla.Core.IPropertyInfo publishedYearProperty) : base(publishedYearProperty)
            {
                InputProperties.Add(PrimaryProperty);
            }
            protected override void Execute(IRuleContext context)
            {
                int? publishedYear = null;
                if (!context.TryGetInputValue(PrimaryProperty, ref publishedYear))
                {
                    return;
                }
                if (publishedYear < 1584)
                {
                    context.AddErrorResult("Leto izdaje knjige ne more biti starejšo od 1584.");
                }
                if (publishedYear > DateTime.Now.Year)
                {
                    context.AddErrorResult("Leto izdaje knjige ne more biti novejšo od tekočega leta.");
                }
            }
        }

        #endregion

        #region Client-side methods

        #endregion

        #region Server-side methods

        #endregion
    }
}
