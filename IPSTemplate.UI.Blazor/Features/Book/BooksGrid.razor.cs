using Core.Library.Base;
using IPSBlazor.Components;
using IPSTemplate.BusinessLibrary.BO.Book;
using IPSTemplate.BusinessLibrary.BO.BookCopy;
using IPSTemplate.Dal.Models;
using IPSTemplate.UI.Blazor.Base;
using Telerik.Blazor.Components;
using Telerik.DataSource;

namespace IPSTemplate.UI.Blazor.Features.Book;

public partial class BooksGrid
    : GridView<BooksGrid,
               TEBookInfo,
               TEBookGridInfo,
               TEBookRL,
               TEBook>

{
    [Parameter] public EventCallback<TEBookInfo> EditClicked { get; set; }
    [Parameter] public EventCallback<TEBookCopyInfo> CopyEditClicked { get; set; }
    [Parameter] public EventCallback<TEBookInfo> AddClicked { get; set; }
    private IPSGrid<TEBookInfo> _ref = default!;



    protected override async Task<TEBookGridInfo> GetGridData(MobileCslaRequest request)
    {
        return await TEBookGridInfo.GetGridInfoAsync(request, DataPortalFactory);
    }

    private async Task OnStateInitHandler(GridStateEventArgs<TEBookInfo> args)
    {
        var state = new GridState<TEBookInfo>
        {
            SortDescriptors = new List<SortDescriptor>
                    {
                        new SortDescriptor{ Member="Title", SortDirection = ListSortDirection.Ascending  },
                    },
            FilterDescriptors = new List<IFilterDescriptor>()
        };
        args.GridState = state;
    }

    //private async Task OnStateInitHandler(GridStateEventArgs<TEBookCopyInfo> args)
    //{
    //    var state = new GridState<TEBookCopyInfo>
    //    {
    //        SortDescriptors = new List<SortDescriptor>
    //                {
    //                    new SortDescriptor{ Member="IsAvailable", SortDirection = ListSortDirection.Ascending  },
    //                },
    //        FilterDescriptors = new List<IFilterDescriptor>()
    //    };
    //    args.GridState = state;
    //}

    public void Rebind() => _ref.Rebind();

}
