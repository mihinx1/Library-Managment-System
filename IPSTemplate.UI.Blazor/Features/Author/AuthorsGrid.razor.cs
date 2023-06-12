using Core.Library.Base;
using IPSBlazor.Components;
using IPSTemplate.BusinessLibrary.BO.Author;
using IPSTemplate.Dal.Models;
using IPSTemplate.UI.Blazor.Base;
using Telerik.Blazor.Components;
using Telerik.DataSource;

namespace IPSTemplate.UI.Blazor.Features.Author;

public partial class AuthorsGrid
    : GridView<AuthorsGrid,
               TEAuthorInfo,
               TEAuthorGridInfo,
               TEAuthorRL,
               TEAuthor>

{
    [Parameter] public EventCallback<TEAuthorInfo> EditClicked { get; set; }
    private IPSGrid<TEAuthorInfo> _ref = default!;

    protected override async Task<TEAuthorGridInfo> GetGridData(MobileCslaRequest request)
    {
        return await TEAuthorGridInfo.GetGridInfoAsync(request, DataPortalFactory);
    }

    private async Task OnStateInitHandler(GridStateEventArgs<TEAuthorInfo> args)
    {
        var state = new GridState<TEAuthorInfo>
        {
            SortDescriptors = new List<SortDescriptor>
                    {
                        new SortDescriptor{ Member="LastName", SortDirection = ListSortDirection.Ascending  }
                    },
            FilterDescriptors = new List<IFilterDescriptor>()
        };
        args.GridState = state;
    }

    public void Rebind() => _ref.Rebind();

}
