using Core.Library.Base;
using Csla;
using IPSTemplate.Dal.Models;
using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.BusinessLibrary.BO.Category
{
    [Serializable]
    public class TECategoryInfo : CslaReadOnlyBase<TECategoryInfo, TECategory>
    {
        #region Properties

        public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);
        [Display(Name = "Kategorija")]
        public string Name
        {
            get => GetProperty(NameProperty);
            set => LoadProperty(NameProperty, value);
        }

        public static readonly PropertyInfo<string> ColorProperty = RegisterProperty<string>(p => p.Color);
        [Display(Name = "Barva Kategorije")]
        public string Color
        {
            get => GetProperty(ColorProperty);
            set => LoadProperty(ColorProperty, value);
        }


        #endregion
    }
}
