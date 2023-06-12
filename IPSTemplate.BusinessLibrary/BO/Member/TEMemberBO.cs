using Core.Library.Base;
using Csla;
using Csla.Rules;
using IPSTemplate.Dal.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IPSTemplate.BusinessLibrary.BO.Member
{
    [Serializable]
    public class TEMemberBO : CslaBusinessBase<TEMemberBO, TEMember>
    {
        #region Properties

        public static readonly PropertyInfo<string> FirstNameProperty = RegisterProperty<string>(p => p.FirstName);
        [Required]
        [LocalizedStringLength(20, 2)]
        [Display(Name = "Ime")]
        public string FirstName
        {
            get => GetProperty(FirstNameProperty);
            set => SetProperty(FirstNameProperty, value);
        }

        public static readonly PropertyInfo<string> LastNameProperty = RegisterProperty<string>(p => p.LastName);
        [Required]
        [LocalizedStringLength(20, 2)]
        [Display(Name = "Priimek")]
        public string LastName
        {
            get => GetProperty(LastNameProperty);
            set => SetProperty(LastNameProperty, value);
        }

        public static readonly PropertyInfo<int> BirthYearProperty = RegisterProperty<int>(p => p.BirthYear);
        [Display(Name = "Letnica rojstva")]
        public int BirthYear
        {
            get => GetProperty(BirthYearProperty);
            set => SetProperty(BirthYearProperty, value);
        }

        public static readonly PropertyInfo<string> NationalityProperty = RegisterProperty<string>(p => p.Nationality);
        [Required]
        [Display(Name = "Nacionalnost")]
        public string Nationality
        {
            get => GetProperty(NationalityProperty);
            set => SetProperty(NationalityProperty, value);
        }

        public static readonly PropertyInfo<string> GenderProperty = RegisterProperty<string>(p => p.Gender);
        [Required]
        [Display(Name = "Spol")]
        public string Gender
        {
            get => GetProperty(GenderProperty);
            set => SetProperty(GenderProperty, value);
        }

        public static readonly PropertyInfo<string> PhoneNumberProperty = RegisterProperty<string>(p => p.PhoneNumber);
        [Display(Name = "Telefonska številka")]
        public string PhoneNumber
        {
            get => GetProperty(PhoneNumberProperty);
            set => SetProperty(PhoneNumberProperty, value);
        }

        public static readonly PropertyInfo<string> EmailProperty = RegisterProperty<string>(p => p.Email);
        [Display(Name = "Email")]
        public string Email
        {
            get => GetProperty(EmailProperty);
            set => SetProperty(EmailProperty, value);
        }


        #endregion

        #region Validation rules

        public async Task CheckRulesAsync() => await BusinessRules.CheckRulesAsync();
        protected override void AddBusinessRules()
        {
            BusinessRules.AddRule(new FirstNameNoNumbersOrSpecialCharactersRule(FirstNameProperty));
            BusinessRules.AddRule(new LastNameNoNumbersOrSpecialCharactersRule(LastNameProperty));
            BusinessRules.AddRule(new OldestAuthorBirthYear(BirthYearProperty));
            BusinessRules.AddRule(new PhoneNumberCorrectFormatRule(PhoneNumberProperty));
            BusinessRules.AddRule(new MemberEmailNoSpecialCharactersRule(EmailProperty));
            base.AddBusinessRules();

        }
        private class OldestAuthorBirthYear : BusinessRule
        {
            public OldestAuthorBirthYear(Csla.Core.IPropertyInfo birthYearProperty) : base(birthYearProperty)
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
                if (!(firstNameProperty == string.Empty))
                {
                    if (!Regex.IsMatch(firstNameProperty, @"^[a-zA-Z0-9\s]+$"))
                    {
                        context.AddErrorResult("Member first name must not contain numbers or special characters.");
                    }
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
                if (!(lastNameProperty == string.Empty))
                {
                    if (!Regex.IsMatch(lastNameProperty, @"^[a-zA-Z0-9\s]+$"))
                    {
                        context.AddErrorResult("Member last name must not contain numbers or special characters.");
                    }
                }
            }
        }



        private class PhoneNumberCorrectFormatRule : BusinessRule
        {
            public PhoneNumberCorrectFormatRule(Csla.Core.IPropertyInfo phoneNumberProperty) : base(phoneNumberProperty)
            {
                InputProperties.Add(PrimaryProperty);
            }

            protected override void Execute(IRuleContext context)
            {
                string? phoneNumberProperty = null;
                if (!context.TryGetInputValue(PrimaryProperty, ref phoneNumberProperty))
                {
                    return;
                }
                if (!(phoneNumberProperty == string.Empty))
                {
                    if (!Regex.IsMatch(phoneNumberProperty, @"^(\+386|0)[1-9]\d{6,7}$"))
                    {
                        context.AddErrorResult("Napačen format telefonske številke.\nPravilen format: +38612345678 ali 03112345678");
                    }
                }
                if (phoneNumberProperty == string.Empty)
                {
                    context.AddErrorResult("Member must have a phone number.");
                }
            }
        }

        private class MemberEmailNoSpecialCharactersRule : BusinessRule
        {
            public MemberEmailNoSpecialCharactersRule(Csla.Core.IPropertyInfo memberEmailProperty) : base(memberEmailProperty)
            {
                InputProperties.Add(PrimaryProperty);
            }

            protected override void Execute(IRuleContext context)
            {
                string? memberEmail = null;

                if (!context.TryGetInputValue(PrimaryProperty, ref memberEmail))
                {
                    return;
                }
                if (!(memberEmail == string.Empty))
                {
                    if (!Regex.IsMatch(memberEmail, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
                    {
                        context.AddErrorResult("Nepravilen email.");
                        return;
                    }
                }
                if (memberEmail == string.Empty)
                {
                    context.AddErrorResult("Member must have email.");
                }
            }

            #endregion

            #region Client-side methods

            #endregion

            #region Server-side methods

            #endregion
        }
    }
}
