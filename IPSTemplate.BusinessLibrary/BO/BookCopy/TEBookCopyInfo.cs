using Core.Library.Base;
using Csla;
using IPSTemplate.Dal.Models;
using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.BusinessLibrary.BO.BookCopy
{
    [Serializable]
    public class TEBookCopyInfo : CslaReadOnlyBase<TEBookCopyInfo, TEBookCopy>
    {
        #region Properties


        //public static readonly PropertyInfo<DateTime> CreationDateProperty = RegisterProperty<DateTime>(p => p.CreationDate);
        //[Display(Name = "Datum Izdelave")]
        //public DateTime CreationDate
        //{
        //    get => GetProperty(CreationDateProperty);
        //    set => LoadProperty(CreationDateProperty, value);
        //}

        public static readonly PropertyInfo<int> PublishedYearProperty = RegisterProperty<int>(p => p.PublishedYear);
        [Display(Name = "Leto izida")]
        public int PublishedYear
        {
            get => GetProperty(PublishedYearProperty);
            set => LoadProperty(PublishedYearProperty, value);
        }

        public static readonly PropertyInfo<Guid> BookIDProperty = RegisterProperty<Guid>(p => p.BookID);
        [Display(Name = "Id Knjige")]
        public Guid BookID
        {
            get => GetProperty(BookIDProperty);
            set => LoadProperty(BookIDProperty, value);
        }

        public static readonly PropertyInfo<Guid> PublisherIDProperty = RegisterProperty<Guid>(p => p.PublisherID);
        [Display(Name = "Id Založbe")]
        public Guid PublisherID
        {
            get => GetProperty(PublisherIDProperty);
            set => LoadProperty(PublisherIDProperty, value);
        }

        public static readonly PropertyInfo<string> PublisherNameProperty = RegisterProperty<string>(p => p.PublisherName);
        [Display(Name = "Naziv Založbe")]
        public string PublisherName
        {
            get => GetProperty(PublisherNameProperty);
            set => LoadProperty(PublisherNameProperty, value);
        }

        public static readonly PropertyInfo<string> BookTitleProperty = RegisterProperty<string>(p => p.BookTitle);
        [Display(Name = "Naslov")]
        public string BookTitle
        {
            get => GetProperty(BookTitleProperty);
            set => LoadProperty(BookTitleProperty, value);
        }

        public static readonly PropertyInfo<bool> IsAvailableProperty = RegisterProperty<bool>(p => p.IsAvailable);
        [Display(Name = "IsAvailable")]
        public bool IsAvailable
        {
            get => GetProperty(IsAvailableProperty);
            set => LoadProperty(IsAvailableProperty, value);
        }
        public string ShowAs
        {
            get => String.Join(" - ", BookTitle, PublishedYear);
        }

        public string Status
        {
            get
            {
                if (IsAvailable)
                {
                    return "Na voljo";
                }
                else
                {
                    return "Izposojeno";
                }
            }
        }


        //public static readonly PropertyInfo<Guid?> PublisherIDProperty = RegisterProperty<Guid?>(p => p.PublisherID);
        //[Display(Name = "Id založbe")]
        //public Guid? PublisherID
        //{
        //    get => GetProperty(PublisherIDProperty);
        //    set => LoadProperty(PublisherIDProperty, value);
        //}

        //protected TEBookAuthorEL GetAuthors(IDataPortalFactory factory)
        //{
        //    return TEBookAuthorEL.GetByBookId(Id, factory);
        //}


        //[Display(Name = "Avtorji")]
        //public string AuthorNames => String.Join(", ", Authors.Select(a => a.ShowAs));


        //public static readonly PropertyInfo<string> PublisherProperty = RegisterProperty<string>(p => p.PublisherFirstName);
        //[Display(Name = "Založba")]
        //public string PublisherFirstName
        //{
        //    get => GetProperty(PublisherProperty);
        //    set => LoadProperty(PublisherProperty, value);
        //}

        protected override void FetchChild(TEBookCopy entity)
        {
            base.FetchChild(entity);
            IsAvailable = entity.Checkouts is null || entity.Checkouts.All(p => p.IsReturned);
        }

        #endregion
    }
}
