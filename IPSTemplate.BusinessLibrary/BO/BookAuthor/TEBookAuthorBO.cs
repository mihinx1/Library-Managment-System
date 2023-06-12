using Core.Library.Base;
using Csla;
using IPSTemplate.BusinessLibrary.BO.Author;
using IPSTemplate.Dal.Models;
using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.BusinessLibrary.BO.Book
{
    [Serializable]
    public class TEBookAuthorBO : CslaBusinessBase<TEBookAuthorBO, TEBookAuthor>
    {
        #region Properties
        public static readonly PropertyInfo<Guid> BookIDProperty = RegisterProperty<Guid>(p => p.BookID);
        [Required]
        public Guid BookID
        {
            get => GetProperty(BookIDProperty);
            set => SetProperty(BookIDProperty, value);
        }

        public static readonly PropertyInfo<Guid> AuthorIDProperty = RegisterProperty<Guid>(p => p.AuthorID);
        [Required]
        public Guid AuthorID
        {
            get => GetProperty(AuthorIDProperty);
            set => SetProperty(AuthorIDProperty, value);
        }

        public static readonly PropertyInfo<TEAuthorInfo> AuthorProperty = RegisterProperty<TEAuthorInfo>(p => p.Author, RelationshipTypes.LazyLoad);
        public TEAuthorInfo Author
        {
            get => LazyGetPropertyAsync(AuthorProperty, FieldManager.FieldExists(AuthorProperty) ? Task.FromResult(default(TEAuthorInfo)) : TEAuthorInfo.GetItemAsync(AuthorID, ApplicationContext.GetRequiredService<IDataPortalFactory>()));
            set => SetProperty(AuthorProperty, value);
        }

        public static readonly PropertyInfo<string> AuthorShowAsProperty = RegisterProperty<string>(p => p.AuthorShowAs);
        [Display(Name = "Avtor")]
        public string AuthorShowAs => string.Format("{0} {1}", Author?.FirstName, Author?.LastName);


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
