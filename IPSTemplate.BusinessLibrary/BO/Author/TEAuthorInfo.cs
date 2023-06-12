using Core.Library.Base;
using Csla;
using IPSTemplate.Dal.Models;
using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.BusinessLibrary.BO.Author
{
    [Serializable]
    public class TEAuthorInfo : CslaReadOnlyBase<TEAuthorInfo, TEAuthor>
    {
        #region Properties

        public static readonly PropertyInfo<string> FirstNameProperty = RegisterProperty<string>(p => p.FirstName);
        [Display(Name = "Ime")]
        public string FirstName
        {
            get => GetProperty(FirstNameProperty);
            set => LoadProperty(FirstNameProperty, value);
        }

        public static readonly PropertyInfo<string> LastNameProperty = RegisterProperty<string>(p => p.LastName);
        [Display(Name = "Priimek")]
        public string LastName
        {
            get => GetProperty(LastNameProperty);
            set => LoadProperty(LastNameProperty, value);
        }

        public static readonly PropertyInfo<string> NationalityProperty = RegisterProperty<string>(p => p.Nationality);
        [Display(Name = "Nacionalnost")]
        public string Nationality
        {
            get => GetProperty(NationalityProperty);
            set => LoadProperty(NationalityProperty, value);
        }

        public static readonly PropertyInfo<int> BirthYearProperty = RegisterProperty<int>(p => p.BirthYear);
        [Display(Name = "Letnica rojstva")]
        public int BirthYear
        {
            get => GetProperty(BirthYearProperty);
            set => LoadProperty(BirthYearProperty, value);
        }

        public string ShowAs
        {
            get => FirstName + " " + LastName;
        }
        #endregion
    }
}
