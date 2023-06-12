using Core.Library.Base;
using Csla;
using IPSTemplate.Dal.Models;
using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.BusinessLibrary.BO.Category
{
    [Serializable]
    public class TECategoryBO : CslaBusinessBase<TECategoryBO, TECategory>
    {
        #region Properties

        public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);
        [Required]
        [LocalizedStringLength(20, 2)]
        [Display(Name = "Ime Kategorije")]
        public string Name
        {
            get => GetProperty(NameProperty);
            set => SetProperty(NameProperty, value);
        }

        public static readonly PropertyInfo<string> ColorProperty = RegisterProperty<string>(p => p.Color);
        [Required]
        [LocalizedStringLength(7)]
        [Display(Name = "Barva Kategorije")]
        public string Color
        {
            get => GetProperty(ColorProperty);
            set => SetProperty(ColorProperty, value);
        }

        #endregion

        #region Validation rules

        public async Task CheckRulesAsync() => await BusinessRules.CheckRulesAsync();

        #endregion

        #region Client-side methods

        #endregion

        #region Server-side methods

        #endregion
    }
}
