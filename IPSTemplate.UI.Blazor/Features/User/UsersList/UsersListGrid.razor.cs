using Core.DAL.Infrastructure;
using Core.Library.Base;
using IPSBlazor.Components;
using IPSTemplate.BusinessLibrary.BO.Identity.User;
using IPSTemplate.UI.Blazor.Base;
using Telerik.Blazor.Components;
using Telerik.DataSource;

namespace IPSTemplate.UI.Blazor.Features.User;

public partial class UsersListGrid
    : GridView<UsersListGrid,
               TEUserInfo,
               TEUserGridInfo,
               TEUserRL,
               Entity>

{
    private IPSGrid<TEUserInfo> _ref = default!;

    [Parameter] public EventCallback<TEUserInfo> EditClicked { get; set; }

    protected override async Task<TEUserGridInfo> GetGridData(MobileCslaRequest request)
    {
        return await TEUserGridInfo.GetGridInfoAsync(request, DataPortalFactory);
    }

    private async Task OnStateInitHandler(GridStateEventArgs<TEUserInfo> args)
    {
        var state = new GridState<TEUserInfo>
        {
            SortDescriptors = new List<SortDescriptor>
                    {
                        new SortDescriptor{ Member="DisplayName", SortDirection = ListSortDirection.Ascending  }
                    },
            FilterDescriptors = new List<IFilterDescriptor>()
        };
        args.GridState = state;
    }

    public void Rebind() => _ref.Rebind();

}
