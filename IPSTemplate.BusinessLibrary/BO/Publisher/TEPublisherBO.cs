using Core.Library.Base;
using Csla;
using Csla.Rules;
using IPSTemplate.Dal.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IPSTemplate.BusinessLibrary.BO.Publisher
{
    [Serializable]
    public class TEPublisherBO : CslaBusinessBase<TEPublisherBO, TEPublisher>
    {
        #region Properties

        public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);
        [Required]
        [LocalizedStringLength(50, 2)]
        [Display(Name = "Naziv Izložbe")]
        public string Name
        {
            get => GetProperty(NameProperty);
            set => SetProperty(NameProperty, value);
        }

        public static readonly PropertyInfo<string> LocationProperty = RegisterProperty<string>(p => p.Location);
        [Required]
        [Display(Name = "Lokacija")]
        public string Location
        {
            get => GetProperty(LocationProperty);
            set => SetProperty(LocationProperty, value);
        }

        public static readonly PropertyInfo<string> EmailProperty = RegisterProperty<string>(p => p.Email);
        [Required]
        [LocalizedStringLength(50, 2)]
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
            BusinessRules.AddRule(new PublisherNameNoNumbersOrSpecialCharactersRule(NameProperty));
            BusinessRules.AddRule(new PublisherEmailNoSpecialCharactersRule(EmailProperty));
            base.AddBusinessRules();
        }

        private class PublisherNameNoNumbersOrSpecialCharactersRule : BusinessRule
        {
            public PublisherNameNoNumbersOrSpecialCharactersRule(Csla.Core.IPropertyInfo publisherNameProperty) : base(publisherNameProperty)
            {
                InputProperties.Add(PrimaryProperty);
            }

            protected override void Execute(IRuleContext context)
            {
                string? publisherName = null;

                if (!context.TryGetInputValue(PrimaryProperty, ref publisherName))
                {
                    return;
                }
                if (!(publisherName == string.Empty))
                {
                    if (!Regex.IsMatch(publisherName, @"^[a-zA-Z0-9\s]+$"))
                    {
                        context.AddErrorResult("Publisher name must not contain numbers or special characters.");
                    }
                }
            }
        }

        private class PublisherEmailNoSpecialCharactersRule : BusinessRule
        {
            public PublisherEmailNoSpecialCharactersRule(Csla.Core.IPropertyInfo publisherEmailProperty) : base(publisherEmailProperty)
            {
                InputProperties.Add(PrimaryProperty);
            }

            protected override void Execute(IRuleContext context)
            {
                string? publisherEmail = null;

                if (!context.TryGetInputValue(PrimaryProperty, ref publisherEmail))
                {
                    return;
                }
                if (!(publisherEmail == string.Empty))
                {
                    if (!Regex.IsMatch(publisherEmail, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
                    {
                        context.AddErrorResult("Nepravilen email.");
                        return;
                    }
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
