using Core.Library.Base;
using Csla;
using IPSTemplate.BusinessLibrary.BO.BookCopy;
using IPSTemplate.Dal.Models;
using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.BusinessLibrary.BO.Checkout
{
    [Serializable]
    public class TECheckoutInfo : CslaReadOnlyBase<TECheckoutInfo, TECheckout>
    {
        #region Properties

        //public static readonly PropertyInfo<string> BookTitleProperty = RegisterProperty<string>(p => p.BookTitle);
        //[Display(Name = "Ime Knjige")]
        //public string BookTitle
        //{
        //    get => GetProperty(BookTitleProperty);
        //    set => LoadProperty(BookTitleProperty, value);
        //}

        //public static readonly PropertyInfo<DateTime> StartDateProperty = RegisterProperty<DateTime>(p => p.StartDate);
        //[Display(Name = "Datum izposoje")]
        //public DateTime StartDate
        //{
        //    get => GetProperty(StartDateProperty);
        //    set => LoadProperty(StartDateProperty, value);
        //}

        public static readonly PropertyInfo<bool> IsAccordingToPlannedDateProperty = RegisterProperty<bool>(p => p.IsAccordingToPlannedDate);
        [Display(Name = "Stanje")]
        public bool IsAccordingToPlannedDate => PlanedDate > DateTime.Now;

        public static readonly PropertyInfo<int> RemainingDaysDateProperty = RegisterProperty<int>(p => p.RemainingDays);
        [Display(Name = "Preostali dni")]
        public int RemainingDays => (int)(PlanedDate! - DateCreated).Value.TotalDays + 1;


        public static readonly PropertyInfo<DateTime> DateCreatedProperty = RegisterProperty<DateTime>(p => p.DateCreated);
        [Display(Name = "Datum izposoje")]
        public DateTime DateCreated
        {
            get => GetProperty(DateCreatedProperty);
            set => LoadProperty(DateCreatedProperty, value);
        }

        public static readonly PropertyInfo<DateTime?> PlanedDateProperty = RegisterProperty<DateTime?>(p => p.PlanedDate);
        [Display(Name = "Planirani datum vrnitve")]
        public DateTime? PlanedDate
        {
            get => GetProperty(PlanedDateProperty);
            set => LoadProperty(PlanedDateProperty, value);
        }

        //public static readonly PropertyInfo<DateTime> EndDateProperty = RegisterProperty<DateTime>(p => p.EndDate);
        //[Display(Name = "Datum vrnitve")]
        //public DateTime EndDate
        //{
        //    get => GetProperty(EndDateProperty);
        //    set => LoadProperty(EndDateProperty, value);
        //}

        // DateChanged --> EndDate
        public static readonly PropertyInfo<DateTime> DateChangedProperty = RegisterProperty<DateTime>(p => p.DateChanged);
        public DateTime DateChanged
        {
            get => GetProperty(DateChangedProperty);
            set => LoadProperty(DateChangedProperty, value);
        }

        public static readonly PropertyInfo<bool> IsReturnedProperty = RegisterProperty<bool>(p => p.IsReturned);
        [Display(Name = "Vrnjeno")]
        public bool IsReturned
        {
            get => GetProperty(IsReturnedProperty);
            set => LoadProperty(IsReturnedProperty, value);
        }

        //public static readonly PropertyInfo<Guid> MemberIDProperty = RegisterProperty<Guid>(p => p.MemberID);
        //[Display(Name = "Id Člana")]
        //public Guid MemberID
        //{
        //    get => GetProperty(MemberIDProperty);
        //    set => LoadProperty(MemberIDProperty, value);
        //}

        public static readonly PropertyInfo<string> BookCopyBookTitleProperty = RegisterProperty<string>(p => p.BookCopyBookTitle);
        [Display(Name = "Ime Knjige")]
        public string BookCopyBookTitle
        {
            get => GetProperty(BookCopyBookTitleProperty);
            set => LoadProperty(BookCopyBookTitleProperty, value);
        }

        //public static readonly PropertyInfo<string> MemberFirstNameProperty = RegisterProperty<string>(p => p.MemberFirstName);
        //public string MemberFirstName
        //{
        //    get => GetProperty(MemberFirstNameProperty);
        //    set => LoadProperty(MemberFirstNameProperty, value);
        //}

        //public static readonly PropertyInfo<string> MemberLastNameProperty = RegisterProperty<string>(p => p.MemberLastName);
        //public string MemberLastName
        //{
        //    get => GetProperty(MemberLastNameProperty);
        //    set => LoadProperty(MemberLastNameProperty, value);
        //}


        public static readonly PropertyInfo<Guid> UserIDProperty = RegisterProperty<Guid>(p => p.UserID);
        [Display(Name = "Id Uporabnika")]
        public Guid UserID
        {
            get => GetProperty(UserIDProperty);
            set => LoadProperty(UserIDProperty, value);
        }

        public static readonly PropertyInfo<string> UserNameProperty = RegisterProperty<string>(p => p.UserName);
        public string UserName
        {
            get => GetProperty(UserNameProperty);
            set => LoadProperty(UserNameProperty, value);
        }

        public static readonly PropertyInfo<string> UserSurnameProperty = RegisterProperty<string>(p => p.UserSurname);
        public string UserSurname
        {
            get => GetProperty(UserSurnameProperty);
            set => LoadProperty(UserSurnameProperty, value);
        }


        [Display(Name = "Uporabnik")]
        public string UserShowAs
        {
            get => String.Join(" ", UserName, UserSurname);
        }

        public static readonly PropertyInfo<Guid> BookCopyIDProperty = RegisterProperty<Guid>(p => p.BookCopyID);
        [Display(Name = "Id Kopije")]
        public Guid BookCopyID
        {
            get => GetProperty(BookCopyIDProperty);
            set => LoadProperty(BookCopyIDProperty, value);
        }

        //public static readonly PropertyInfo<bool> BookReturnedProperty = RegisterProperty<bool>(p => p.BookReturned);
        //[Display(Name = "Vrnjena Knjiga")]
        //public bool BookReturned
        //{
        //    get => GetProperty(BookReturnedProperty);
        //    set => LoadProperty(BookReturnedProperty, value);
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
