using Core.Library.Base;
using Csla;
using IPSTemplate.Dal.Models;
using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.BusinessLibrary.BO.Publisher
{
    [Serializable]
    public class TEPublisherInfo : CslaReadOnlyBase<TEPublisherInfo, TEPublisher>
    {
        #region Properties

        public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);
        [Display(Name = "Naziv Izložbe")]
        public string Name
        {
            get => GetProperty(NameProperty);
            set => LoadProperty(NameProperty, value);
        }

        public static readonly PropertyInfo<string> LocationProperty = RegisterProperty<string>(p => p.Location);
        [Display(Name = "Lokacija")]
        public string Location
        {
            get => GetProperty(LocationProperty);
            set => LoadProperty(LocationProperty, value);
        }

        public static readonly PropertyInfo<string> EmailProperty = RegisterProperty<string>(p => p.Email);
        [Display(Name = "Email")]
        public string Email
        {
            get => GetProperty(EmailProperty);
            set => LoadProperty(EmailProperty, value);
        }

        #endregion
    }
}
