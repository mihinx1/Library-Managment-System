using Core.Library.Base;
using Csla;
using Csla.Rules;
using IPSTemplate.Dal.Models;
using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.BusinessLibrary.BO.Checkout
{
    [Serializable]
    public class TECheckoutBO : CslaBusinessBase<TECheckoutBO, TECheckout>
    {
        #region Properties

        public static readonly PropertyInfo<DateTime?> PlanedDateProperty = RegisterProperty<DateTime?>(p => p.PlanedDate);
        [Required]
        [Display(Name = "Planirani datum vrnitve")]
        public DateTime? PlanedDate
        {
            get => GetProperty(PlanedDateProperty);
            set => SetProperty(PlanedDateProperty, value);
        }

        public static readonly PropertyInfo<DateTime> EndDateProperty = RegisterProperty<DateTime>(p => p.EndDate);
        [Required]
        [Display(Name = "Datum vrnitve")]
        public DateTime EndDate
        {
            get => GetProperty(EndDateProperty);
            set => SetProperty(EndDateProperty, value);
        }

        public static readonly PropertyInfo<bool> IsReturnedProperty = RegisterProperty<bool>(p => p.IsReturned);
        [Required]
        [Display(Name = "Vrnjeno")]
        public bool IsReturned
        {
            get => GetProperty(IsReturnedProperty);
            set => SetProperty(IsReturnedProperty, value);
        }

        //public static readonly PropertyInfo<Guid> MemberIDProperty = RegisterProperty<Guid>(p => p.MemberID);
        //[Required]
        //[Display(Name = "Id Člana")]
        //public Guid MemberID
        //{
        //    get => GetProperty(MemberIDProperty);
        //    set => SetProperty(MemberIDProperty, value);
        //}

        public static readonly PropertyInfo<Guid> UserIDProperty = RegisterProperty<Guid>(p => p.UserID);
        [Required]
        [Display(Name = "Id Uporabnika")]
        public Guid UserID
        {
            get => GetProperty(UserIDProperty);
            set => SetProperty(UserIDProperty, value);
        }

        public static readonly PropertyInfo<Guid> BookCopyIDProperty = RegisterProperty<Guid>(p => p.BookCopyID);
        [Required]
        [Display(Name = "Id Kopije")]
        public Guid BookCopyID
        {
            get => GetProperty(BookCopyIDProperty);
            set => SetProperty(BookCopyIDProperty, value);
        }

        public static readonly PropertyInfo<string> BookCopyBookTitleProperty = RegisterProperty<string>(p => p.BookCopyBookTitle);
        [Display(Name = "Ime Knjige")]
        public string BookCopyBookTitle
        {
            get => GetProperty(BookCopyBookTitleProperty);
            set => SetProperty(BookCopyBookTitleProperty, value);
        }

        //public static readonly PropertyInfo<bool> BookReturnedProperty = RegisterProperty<bool>(p => p.BookReturned);
        //[Display(Name = "Vrnjena knjiga")]
        //public bool BookReturned
        //{
        //    get => GetProperty(BookReturnedProperty);
        //    set => SetProperty(BookReturnedProperty, value);
        //}

        public void OnReturn()
        {
            IsReturned = true;
        }

        #endregion

        #region Validation rules

        public async Task CheckRulesAsync() => await BusinessRules.CheckRulesAsync();
        protected override void AddBusinessRules()
        {
            BusinessRules.AddRule(new HasMemberRule(UserIDProperty));
            BusinessRules.AddRule(new HasBookCopyRule(BookCopyIDProperty));
            base.AddBusinessRules();

        }

        private class HasMemberRule : BusinessRule
        {
            public HasMemberRule(Csla.Core.IPropertyInfo membersProperty) : base(membersProperty)
            {
                InputProperties.Add(PrimaryProperty);
            }
            protected override void Execute(IRuleContext context)
            {
                Guid? membersProperty = null;
                if (!context.TryGetInputValue(PrimaryProperty, ref membersProperty))
                {
                    return;
                }
                if (membersProperty == Guid.Empty)
                {
                    context.AddErrorResult("Član is required.");
                }
            }
        }
        private class HasBookCopyRule : BusinessRule
        {
            public HasBookCopyRule(Csla.Core.IPropertyInfo bookCopyProperty) : base(bookCopyProperty)
            {
                InputProperties.Add(PrimaryProperty);
            }
            protected override void Execute(IRuleContext context)
            {
                Guid? bookCopyProperty = null;
                if (!context.TryGetInputValue(PrimaryProperty, ref bookCopyProperty))
                {
                    return;
                }
                if (bookCopyProperty == Guid.Empty)
                {
                    context.AddErrorResult("Kopija is required.");
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
