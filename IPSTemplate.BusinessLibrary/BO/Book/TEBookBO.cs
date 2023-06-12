using Core.Library.Base;
using Csla;
using Csla.Rules;
using IPSTemplate.Dal.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IPSTemplate.BusinessLibrary.BO.Book
{
    [Serializable]
    public class TEBookBO : CslaBusinessBase<TEBookBO, TEBook>
    {
        #region Properties

        public static readonly PropertyInfo<string> TitleProperty = RegisterProperty<string>(p => p.Title);
        [Required]
        [LocalizedStringLength(50, 2)]
        [Display(Name = "Naziv Knjige")]
        public string Title
        {
            get => GetProperty(TitleProperty);
            set => SetProperty(TitleProperty, value);
        }

        public static readonly PropertyInfo<string> LanguageProperty = RegisterProperty<string>(p => p.Language);
        [Required]
        [Display(Name = "Jezik")]
        public string Language
        {
            get => GetProperty(LanguageProperty);
            set => SetProperty(LanguageProperty, value);
        }

        public static readonly PropertyInfo<DateTime?> ReleaseDateProperty = RegisterProperty<DateTime?>(p => p.ReleaseDate);
        [Required]
        [Display(Name = "Datum izida")]
        public DateTime? ReleaseDate
        {
            get => GetProperty(ReleaseDateProperty);
            set => SetProperty(ReleaseDateProperty, value);
        }

        public static readonly PropertyInfo<int> TotalPagesProperty = RegisterProperty<int>(p => p.TotalPages);
        [Display(Name = "Število strani")]
        public int TotalPages
        {
            get => GetProperty(TotalPagesProperty);
            set => SetProperty(TotalPagesProperty, value);
        }

        public static readonly PropertyInfo<Guid> CategoryIDProperty = RegisterProperty<Guid>(p => p.CategoryID);
        [Display(Name = "Id Kategorije")]
        public Guid CategoryID
        {
            get => GetProperty(CategoryIDProperty);
            set => SetProperty(CategoryIDProperty, value);
        }

        public static readonly PropertyInfo<string> CategoryNameProperty = RegisterProperty<string>(p => p.CategoryName);
        [Display(Name = "Ime Kategorije")]
        public string CategoryName
        {
            get => GetProperty(CategoryNameProperty);
            set => SetProperty(CategoryNameProperty, value);
        }

        public static readonly PropertyInfo<string> CategoryColorProperty = RegisterProperty<string>(p => p.CategoryColor);
        [Display(Name = "Barva Kategorije")]
        public string CategoryColor
        {
            get => GetProperty(CategoryColorProperty);
            set => SetProperty(CategoryColorProperty, value);
        }

        public static readonly PropertyInfo<Guid> PublisherIDProperty = RegisterProperty<Guid>(p => p.PublisherID);
        [Display(Name = "Založba")]
        public Guid PublisherID
        {
            get => GetProperty(PublisherIDProperty);
            set => SetProperty(PublisherIDProperty, value);
        }

        //public static readonly PropertyInfo<Guid?> AuthorIDProperty = RegisterProperty<Guid?>(p => p.AuthorID);
        //[Display(Name = "Id Autorja")]
        //public Guid? AuthorID
        //{
        //    get => GetProperty(AuthorIDProperty);
        //    set => SetProperty(AuthorIDProperty, value);
        //}

        //public static readonly PropertyInfo<Guid?> PublisherIDProperty = RegisterProperty<Guid?>(p => p.PublisherID);
        //[Display(Name = "Id založbe")]
        //public Guid? PublisherID
        //{
        //    get => GetProperty(PublisherIDProperty);
        //    set => SetProperty(PublisherIDProperty, value);
        //}

        public static readonly PropertyInfo<TEBookAuthorEL> AuthorsProperty = RegisterProperty<TEBookAuthorEL>(p => p.Authors, RelationshipTypes.LazyLoad);
        public TEBookAuthorEL Authors
        {
            get => LazyGetProperty(AuthorsProperty, () => GetAuthors(ApplicationContext.GetRequiredService<IDataPortalFactory>()));
            set => SetProperty(AuthorsProperty, value);
        }
        protected TEBookAuthorEL GetAuthors(IDataPortalFactory factory)
        {
            return TEBookAuthorEL.GetByBookId(Id, factory);
        }

        public static readonly PropertyInfo<List<Guid>> AuthorIdsProperty = RegisterProperty<List<Guid>>(p => p.AuthorIds);
        [Display(Name = "Avtorji")]
        public List<Guid> AuthorIds
        {
            get { return Authors.Any() ? Authors.Select(p => p.AuthorID).ToList() : new List<Guid>(); }
            set
            {
                foreach (var authorId in value.Where(p => !Authors.Select(o => o.AuthorID).ToList().Contains(p)).ToList())
                {
                    var author = ApplicationContext.GetRequiredService<IDataPortalFactory>().GetPortal<TEBookAuthorBO>().Create();
                    author.BookID = Id;
                    author.AuthorID = authorId;
                    Authors.Add(author);
                    MarkDirty();
                    PropertyHasChanged(AuthorsProperty);
                }
                foreach (var item in Authors.Where(p => !value.Contains(p.AuthorID)).ToList())
                {
                    Authors.Remove(item);
                    MarkDirty();
                    PropertyHasChanged(AuthorsProperty);
                }
            }
        }

        #endregion

        #region Validation rules
        public async Task CheckRulesAsync() => await BusinessRules.CheckRulesAsync();
        protected override void AddBusinessRules()
        {
            BusinessRules.AddRule(new HasAtLeastOneAuthorRule(AuthorIdsProperty));
            BusinessRules.AddRule(new BookTitleNoNumbersOrSpecialCharactersRule(TitleProperty));
            BusinessRules.AddRule(new MinAndMaxNumberOfBookPagesRule(TotalPagesProperty));
            BusinessRules.AddRule(new HasCategoryRule(CategoryIDProperty));
            //BusinessRules.AddRule(new ReleaseDateCantBeInFuture(ReleaseDateProperty));
            base.AddBusinessRules();
        }


        private class HasAtLeastOneAuthorRule : BusinessRule
        {
            public HasAtLeastOneAuthorRule(Csla.Core.IPropertyInfo authorIdsProperty) : base(authorIdsProperty)
            {
                InputProperties.Add(PrimaryProperty);
                InputProperties.Add(AuthorsProperty);
            }
            protected override void Execute(IRuleContext context)
            {
                TEBookAuthorEL? authors = null;
                if (!context.TryGetInputValue(AuthorsProperty, ref authors))
                {
                    return;
                }
                if (authors is null || authors.Count < 1)
                {
                    context.AddErrorResult("At least one author is required.");
                }
            }
        }

        private class BookTitleNoNumbersOrSpecialCharactersRule : BusinessRule
        {
            public BookTitleNoNumbersOrSpecialCharactersRule(Csla.Core.IPropertyInfo bookTitleProperty) : base(bookTitleProperty)
            {
                InputProperties.Add(PrimaryProperty);
            }

            protected override void Execute(IRuleContext context)
            {
                string? bookTitle = null;

                if (!context.TryGetInputValue(PrimaryProperty, ref bookTitle))
                {
                    return;
                }
                if (!(bookTitle == string.Empty))
                {
                    if (!Regex.IsMatch(bookTitle, @"^[a-zA-Z0-9\s]+$"))
                    {
                        context.AddErrorResult("Book title must not contain special characters.");
                    }
                }
            }
        }

        private class MinAndMaxNumberOfBookPagesRule : BusinessRule
        {
            public MinAndMaxNumberOfBookPagesRule(Csla.Core.IPropertyInfo bookPagesProperty) : base(bookPagesProperty)
            {
                InputProperties.Add(PrimaryProperty);
            }
            protected override void Execute(IRuleContext context)
            {
                int? bookPagesProperty = null;
                if (!context.TryGetInputValue(PrimaryProperty, ref bookPagesProperty))
                {
                    return;
                }
                if (bookPagesProperty < 2)
                {
                    context.AddErrorResult("Book must contain at least 2 pages.");
                    return;
                }
                if (bookPagesProperty > 2000)
                {
                    context.AddErrorResult("Book must not contain more than 2000 pages.");
                }
            }
        }
        private class HasCategoryRule : BusinessRule
        {
            public HasCategoryRule(Csla.Core.IPropertyInfo categoryProperty) : base(categoryProperty)
            {
                InputProperties.Add(PrimaryProperty);
            }
            protected override void Execute(IRuleContext context)
            {
                Guid? categoryProperty = null;
                if (!context.TryGetInputValue(PrimaryProperty, ref categoryProperty))
                {
                    return;
                }
                if (categoryProperty == Guid.Empty)
                {
                    context.AddErrorResult("Category is required.");
                    return;
                }
            }
        }
        //private class ReleaseDateCantBeInFuture : BusinessRule
        //{
        //    public ReleaseDateCantBeInFuture(Csla.Core.IPropertyInfo releaseDateProperty) : base(releaseDateProperty)
        //    {
        //        InputProperties.Add(PrimaryProperty);
        //    }
        //    protected override void Execute(IRuleContext context)
        //    {
        //        DateTime? releaseDateProperty = null;
        //        if (!context.TryGetInputValue(PrimaryProperty, ref releaseDateProperty))
        //        {
        //            return;
        //        }
        //        if (releaseDateProperty == null)
        //        {
        //            return;
        //        }
        //        if (releaseDateProperty > DateTime.Now)
        //        {
        //            context.AddErrorResult("Datum izida knjige ne more biti novejši od današnjega.");
        //        }
        //    }
        //}

        #endregion

        #region Client-side methods

        protected override async Task<TEBookBO> SaveAsync(bool forceUpdate, object userState, bool isSync)
        {
            return await base.SaveAsync(forceUpdate, userState, isSync);
        }

        #endregion

        #region Server-side methods

        #endregion
    }
}
