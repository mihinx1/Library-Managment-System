using Core.Library.Base;
using IPSBlazor;
using IPSBlazor.Components;
using IPSTemplate.BusinessLibrary.BO.Member;
using IPSTemplate.Dal.Models;
using IPSTemplate.UI.Blazor.Base;

namespace IPSTemplate.UI.Blazor.Features.Member;

public partial class MembersGrid
    : GridView<MembersGrid,
               TEMemberInfo,
               TEMemberGridInfo,
               TEMemberRL,
               TEMember>

{
    [Parameter] public EventCallback<TEMemberInfo> EditClicked { get; set; }
    private IPSGrid<TEMemberInfo> _ref = default!;

    protected override async Task<TEMemberGridInfo> GetGridData(MobileCslaRequest request)
    {
        return await TEMemberGridInfo.GetGridInfoAsync(request, DataPortalFactory);
    }


    public Color GetColor(string gender)
    {
        switch (gender)
        {
            case ("M"):
                return Color.Primary;
                break;
            case ("F"):
                return Color.Danger;
                break;
            case ("Other"):
                return Color.Secondary;
                break;
            default:
                return Color.Success;
        }
    }

    public void Rebind() => _ref.Rebind();
}
