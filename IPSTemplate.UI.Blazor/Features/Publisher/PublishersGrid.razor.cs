using Core.Library.Base;
using IPSBlazor.Components;
using IPSTemplate.BusinessLibrary.BO.Publisher;
using IPSTemplate.Dal.Models;
using IPSTemplate.UI.Blazor.Base;
using Telerik.Blazor.Components;
using Telerik.DataSource;

namespace IPSTemplate.UI.Blazor.Features.Publisher;

public partial class PublishersGrid
    : GridView<PublishersGrid,
               TEPublisherInfo,
               TEPublisherGridInfo,
               TEPublisherRL,
               TEPublisher>

{
    private IPSGrid<TEPublisherInfo> _ref = default!;

    [Parameter] public EventCallback<TEPublisherInfo> EditClicked { get; set; }

    protected override async Task<TEPublisherGridInfo> GetGridData(MobileCslaRequest request)
    {
        return await TEPublisherGridInfo.GetGridInfoAsync(request, DataPortalFactory);
    }

    private async Task OnStateInitHandler(GridStateEventArgs<TEPublisherInfo> args)
    {
        var state = new GridState<TEPublisherInfo>
        {
            SortDescriptors = new List<SortDescriptor>
                    {
                        new SortDescriptor{ Member="Name", SortDirection = ListSortDirection.Ascending  }
                    },
            FilterDescriptors = new List<IFilterDescriptor>()
        };
        args.GridState = state;
    }

    public void Rebind() => _ref.Rebind();

}
