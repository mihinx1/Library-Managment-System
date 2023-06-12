using IPSBlazor.Components;
using IPSBlazor.Extensions;
using IPSTemplate.BusinessLibrary.BO.Book;
using IPSTemplate.BusinessLibrary.BO.BookCopy;
using IPSTemplate.BusinessLibrary.BO.Publisher;
using IPSTemplate.Dal.Models;
using IPSTemplate.UI.Blazor.Base;
using Telerik.Blazor.Components;

namespace IPSTemplate.UI.Blazor.Features.BookCopy
{
    public partial class BookCopyEdit : EditView<TEBookCopyBO, TEBookCopy>
    {
        [Parameter] public EventCallback ItemSaved { get; set; }

        [Parameter] public Guid BookId { get; set; }

        //[Inject] NavigationManager NavigationManager { get; set; } = default!;

        [Inject] protected IDataPortalFactory DataPortalFactory { get; set; } = default!;

        protected IPSComboBox<Guid, TEBookInfo>? cbxBook = default!;
        protected IPSComboBox<Guid, TEPublisherInfo>? cbxPublisher = default!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            ViewModel.Model.BookID = BookId;
        }

        //protected async Task GetAuthors(ReadEventArgs args)
        //{
        //    string? filter = args.Request.GetSingleFilter();
        //    //var selectedIds = new List<Guid>();
        //    //if (ViewModel.Model.AuthorIds?.Any())
        //    //{
        //    //    selectedIds.AddRange(ViewModel.Model.AuthorIds);
        //    //}
        //    var authors = await TEAuthorRL.GetFilteredList(filter ?? "", DataPortalFactory);
        //    args.Data = authors;
        //    args.Total = authors.Count;
        //}

        protected async Task GetPublishers(ReadEventArgs args)
        {
            string? filter = args.Request.GetSingleFilter();
            //var selectedIds = new List<Guid>();
            //if (ViewModel.Model.PublisherID.HasValue)
            //{
            //    selectedIds.Add(ViewModel.Model.PublisherID.Value);
            //}
            var publishers = await TEPublisherRL.GetFilteredList(filter ?? "", DataPortalFactory);
            args.Data = publishers;
            args.Total = publishers.Count;
        }

        protected async Task GetBooks(ReadEventArgs args)
        {
            string? filter = args.Request.GetSingleFilter();
            //var selectedIds = new List<Guid>();
            //if (ViewModel.Model.GenreID.HasValue)
            //{
            //    selectedIds.Add(ViewModel.Model.GenreID.Value);
            //}
            var books = await TEBookRL.GetFilteredList(filter ?? "", DataPortalFactory);
            args.Data = books;
            args.Total = books.Count;
        }

    }
}
