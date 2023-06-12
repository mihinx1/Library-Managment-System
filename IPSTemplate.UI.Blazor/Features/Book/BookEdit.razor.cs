using IPSBlazor.Components;
using IPSBlazor.Extensions;
using IPSTemplate.BusinessLibrary.BO.Author;
using IPSTemplate.BusinessLibrary.BO.Book;
using IPSTemplate.BusinessLibrary.BO.Category;
using IPSTemplate.Dal.Models;
using IPSTemplate.UI.Blazor.Base;
using Telerik.Blazor.Components;
namespace IPSTemplate.UI.Blazor.Features.Book
{
    public partial class BookEdit : EditView<TEBookBO, TEBook>
    {
        [Parameter] public EventCallback ItemSaved { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; } = default!;

        [Inject] protected IDataPortalFactory DataPortalFactory { get; set; } = default!;


        protected IPSComboBox<Guid?, TEAuthorInfo>? cbxAuthor = default!;
        protected async Task GetAuthors(ReadEventArgs args)
        {
            string? filter = args.Request.GetSingleFilter();
            var selectedIds = new List<Guid>();
            //if (ViewModel.Model.AuthorID.HasValue)
            //{
            //    selectedIds.Add(ViewModel.Model.AuthorID.Value);
            //}
            var authors = await TEAuthorRL.GetFilteredList(filter ?? "", DataPortalFactory);
            args.Data = authors;
            args.Total = authors.Count;
        }

        protected IPSComboBox<Guid, TECategoryInfo>? cbxCategory = default!;
        protected async Task GetCategories(ReadEventArgs args)
        {
            string? filter = args.Request.GetSingleFilter();
            var categories = await TECategoryRL.GetFilteredList(filter ?? "", DataPortalFactory);
            args.Data = categories;
            args.Total = categories.Count;
        }
    }
}
