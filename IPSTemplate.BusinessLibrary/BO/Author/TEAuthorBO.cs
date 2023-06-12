using Core.Library.Base;
using Csla;
using Csla.Rules;
using IPSTemplate.Dal.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IPSTemplate.BusinessLibrary.BO.Author
{
    [Serializable]
    public class TEAuthorBO : CslaBusinessBase<TEAuthorBO, TEAuthor>
    {
        #region Properties

        public static readonly PropertyInfo<string> FirstNameProperty = RegisterProperty<string>(p => p.FirstName);
        [Required]
        [LocalizedStringLength(100, 2)]
        [Display(Name = "Ime")]
        public string FirstName
        {
            get => GetProperty(FirstNameProperty);
            set => SetProperty(FirstNameProperty, value);
        }

        public static readonly PropertyInfo<string> LastNameProperty = RegisterProperty<string>(p => p.LastName);
        [Required]
        [LocalizedStringLength(100, 2)]
        [Display(Name = "Priimek")]
        public string LastName
        {
            get => GetProperty(LastNameProperty);
            set => SetProperty(LastNameProperty, value);
        }

        public static readonly PropertyInfo<string> NationalityProperty = RegisterProperty<string>(p => p.Nationality);
        [Required]
        [Display(Name = "Nacionalnost")]
        public string Nationality
        {
            get => GetProperty(NationalityProperty);
            set => SetProperty(NationalityProperty, value);
        }

        public static readonly PropertyInfo<int> BirthYearProperty = RegisterProperty<int>(p => p.BirthYear);
        [Display(Name = "Letnica rojstva")]
        public int BirthYear
        {
            get => GetProperty(BirthYearProperty);
            set => SetProperty(BirthYearProperty, value);
        }

        #endregion

        #region Validation rules

        public async Task CheckRulesAsync() => await BusinessRules.CheckRulesAsync();
        protected override void AddBusinessRules()
        {
            BusinessRules.AddRule(new FirstNameNoNumbersOrSpecialCharactersRule(FirstNameProperty));
            BusinessRules.AddRule(new LastNameNoNumbersOrSpecialCharactersRule(LastNameProperty));
            BusinessRules.AddRule(new OldestAuthorBirthYearRule(BirthYearProperty));
            base.AddBusinessRules();

        }

        private class FirstNameNoNumbersOrSpecialCharactersRule : BusinessRule
        {
            public FirstNameNoNumbersOrSpecialCharactersRule(Csla.Core.IPropertyInfo firstNameProperty) : base(firstNameProperty)
            {
                InputProperties.Add(PrimaryProperty);
            }

            protected override void Execute(IRuleContext context)
            {
                string? firstNameProperty = null;

                if (!context.TryGetInputValue(PrimaryProperty, ref firstNameProperty))
                {
                    return;
                }
                if (Regex.IsMatch(firstNameProperty, @"[\d\W]"))
                {
                    context.AddErrorResult("Ime must not contain numbers or special characters.");
                }

            }
        }

        private class LastNameNoNumbersOrSpecialCharactersRule : BusinessRule
        {
            public LastNameNoNumbersOrSpecialCharactersRule(Csla.Core.IPropertyInfo lastNameProperty) : base(lastNameProperty)
            {
                InputProperties.Add(PrimaryProperty);
            }

            protected override void Execute(IRuleContext context)
            {
                string? lastNameProperty = null;
                if (!context.TryGetInputValue(PrimaryProperty, ref lastNameProperty))
                {
                    return;
                }
                if (Regex.IsMatch(lastNameProperty, @"[\d\W]"))
                {
                    context.AddErrorResult("Priimek must not contain numbers or special characters.");
                }

            }
        }

        private class OldestAuthorBirthYearRule : BusinessRule
        {
            public OldestAuthorBirthYearRule(Csla.Core.IPropertyInfo birthYearProperty) : base(birthYearProperty)
            {
                InputProperties.Add(PrimaryProperty);
            }
            protected override void Execute(IRuleContext context)
            {
                int? birthYear = null;
                if (!context.TryGetInputValue(PrimaryProperty, ref birthYear))
                {
                    return;
                }
                if (birthYear == 0)
                {
                    return;
                }
                if (birthYear < DateTime.Now.Year - 120)
                {
                    context.AddErrorResult("Author must not be older than 120 years.");
                }
                if (birthYear > DateTime.Now.Year - 18)
                {
                    context.AddErrorResult("Author must be legal of age.");
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
