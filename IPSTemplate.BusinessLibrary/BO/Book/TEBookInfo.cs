using Core.Library.Base;
using Csla;
using IPSTemplate.BusinessLibrary.BO.BookCopy;
using IPSTemplate.Dal.Models;
using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.BusinessLibrary.BO.Book
{
    [Serializable]
    public class TEBookInfo : CslaReadOnlyBase<TEBookInfo, TEBook>
    {
        #region Properties

        public static readonly PropertyInfo<string> TitleProperty = RegisterProperty<string>(p => p.Title);
        [Display(Name = "Naziv Knjige")]
        public string Title
        {
            get => GetProperty(TitleProperty);
            set => LoadProperty(TitleProperty, value);
        }

        public static readonly PropertyInfo<string> LanguageProperty = RegisterProperty<string>(p => p.Language);
        [Display(Name = "Jezik")]
        public string Language
        {
            get => GetProperty(LanguageProperty);
            set => LoadProperty(LanguageProperty, value);
        }

        public static readonly PropertyInfo<DateTime?> ReleaseDateProperty = RegisterProperty<DateTime?>(p => p.ReleaseDate);
        [Display(Name = "Datum izida")]
        public DateTime? ReleaseDate
        {
            get => GetProperty(ReleaseDateProperty);
            set => LoadProperty(ReleaseDateProperty, value);
        }

        public static readonly PropertyInfo<int> TotalPagesProperty = RegisterProperty<int>(p => p.TotalPages);
        [Display(Name = "Število strani")]
        public int TotalPages
        {
            get => GetProperty(TotalPagesProperty);
            set => LoadProperty(TotalPagesProperty, value);
        }

        public static readonly PropertyInfo<Guid> CategoryIDProperty = RegisterProperty<Guid>(p => p.CategoryID);
        [Display(Name = "Id Kategorije")]
        public Guid CategoryID
        {
            get => GetProperty(CategoryIDProperty);
            set => LoadProperty(CategoryIDProperty, value);
        }

        public static readonly PropertyInfo<string> CategoryNameProperty = RegisterProperty<string>(p => p.CategoryName);
        [Display(Name = "Ime Kategorije")]
        public string CategoryName
        {
            get => GetProperty(CategoryNameProperty);
            set => LoadProperty(CategoryNameProperty, value);
        }

        public static readonly PropertyInfo<string> CategoryColorProperty = RegisterProperty<string>(p => p.CategoryColor);
        [Display(Name = "Barva Kategorije")]
        public string CategoryColor
        {
            get => GetProperty(CategoryColorProperty);
            set => LoadProperty(CategoryColorProperty, value);
        }

        public static readonly PropertyInfo<Guid?> AuthorIDProperty = RegisterProperty<Guid?>(p => p.AuthorID);
        [Display(Name = "Avtorjev Id")]
        public Guid? AuthorID
        {
            get => GetProperty(AuthorIDProperty);
            set => LoadProperty(AuthorIDProperty, value);
        }

        //public static readonly PropertyInfo<Guid?> PublisherIDProperty = RegisterProperty<Guid?>(p => p.PublisherID);
        //[Display(Name = "Id založbe")]
        //public Guid? PublisherID
        //{
        //    get => GetProperty(PublisherIDProperty);
        //    set => LoadProperty(PublisherIDProperty, value);
        //}

        public static readonly PropertyInfo<string> AuthorFirstNameProperty = RegisterProperty<string>(p => p.AuthorFirstName);
        public string AuthorFirstName
        {
            get => GetProperty(AuthorFirstNameProperty);
            set => LoadProperty(AuthorFirstNameProperty, value);
        }
        public static readonly PropertyInfo<string> AuthorLastNameProperty = RegisterProperty<string>(p => p.AuthorLastName);
        public string AuthorLastName
        {
            get => GetProperty(AuthorLastNameProperty);
            set => LoadProperty(AuthorLastNameProperty, value);
        }

        [Display(Name = "Avtor")]
        public string AuthorFullName
        {
            get => AuthorFirstName + " " + AuthorLastName;
        }

        public static readonly PropertyInfo<TEBookAuthorEL> BookAuthorsProperty = RegisterProperty<TEBookAuthorEL>(p => p.BookAuthors, RelationshipTypes.LazyLoad);
        public TEBookAuthorEL BookAuthors
        {
            get => LazyGetProperty(BookAuthorsProperty, () => GetAuthors(ApplicationContext.GetRequiredService<IDataPortalFactory>()));
            set => LoadProperty(BookAuthorsProperty, value);
        }

        public static readonly PropertyInfo<TEAuthorRL> AuthorsProperty = RegisterProperty<TEAuthorRL>(p => p.Authors, RelationshipTypes.LazyLoad);
        public TEAuthorRL Authors
        {
            get => LazyGetProperty(AuthorsProperty, () => TEAuthorRL.GetListByIds(BookAuthors.Select(ba => ba.AuthorID), ApplicationContext.GetRequiredService<IDataPortalFactory>()));
            set => LoadProperty(AuthorsProperty, value);
        }

        protected TEBookAuthorEL GetAuthors(IDataPortalFactory factory)
        {
            return TEBookAuthorEL.GetByBookId(Id, factory);
        }


        [Display(Name = "Avtorji")]
        public string AuthorNames => String.Join(", ", Authors.Select(a => a.ShowAs));


        //public static readonly PropertyInfo<string> PublisherProperty = RegisterProperty<string>(p => p.PublisherFirstName);
        //[Display(Name = "Založba")]
        //public string PublisherFirstName
        //{
        //    get => GetProperty(PublisherProperty);
        //    set => LoadProperty(PublisherProperty, value);
        //}

        public static readonly PropertyInfo<TEBookCopyRL> BookCopiesProperty = RegisterProperty<TEBookCopyRL>(p => p.BookCopies, RelationshipTypes.LazyLoad);
        public TEBookCopyRL BookCopies
        {
            get => LazyGetProperty(BookCopiesProperty, () => GetBookCopies(ApplicationContext.GetRequiredService<IDataPortalFactory>()));
            set => LoadProperty(BookCopiesProperty, value);
        }

        protected TEBookCopyRL GetBookCopies(IDataPortalFactory factory)
        {
            return TEBookCopyRL.GetByBookId(Id, factory);
        }

        public static readonly PropertyInfo<TEBookCopyRL> ThisBookCopiesProperty = RegisterProperty<TEBookCopyRL>(p => p.ThisBookCopies, RelationshipTypes.LazyLoad);
        public TEBookCopyRL ThisBookCopies
        {
            get => LazyGetProperty(ThisBookCopiesProperty, () => TEBookCopyRL.GetListByIds(BookCopies.Select(ba => ba.BookID), ApplicationContext.GetRequiredService<IDataPortalFactory>()));
            set => LoadProperty(ThisBookCopiesProperty, value);
        }

        #endregion
    }
}
