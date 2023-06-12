using Core.Library.Base;
using IPSBlazor.Components;
using IPSTemplate.BusinessLibrary.BO.Category;
using IPSTemplate.Dal.Models;
using IPSTemplate.UI.Blazor.Base;
using Telerik.Blazor.Components;
using Telerik.DataSource;

namespace IPSTemplate.UI.Blazor.Features.Category;

public partial class CategoriesGrid
    : GridView<CategoriesGrid,
               TECategoryInfo,
               TECategoryGridInfo,
               TECategoryRL,
               TECategory>

{
    [Parameter] public EventCallback<TECategoryInfo> EditClicked { get; set; }
    private IPSGrid<TECategoryInfo> _ref = default!;

    protected override async Task<TECategoryGridInfo> GetGridData(MobileCslaRequest request)
    {
        return await TECategoryGridInfo.GetGridInfoAsync(request, DataPortalFactory);
    }

    private async Task OnStateInitHandler(GridStateEventArgs<TECategoryInfo> args)
    {
        var state = new GridState<TECategoryInfo>
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
