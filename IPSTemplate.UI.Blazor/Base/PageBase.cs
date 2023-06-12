using Microsoft.Extensions.Localization;

namespace IPSTemplate.UI.Blazor.Base;

public abstract class PageBase : ComponentBase
{
    [Inject] internal IStringLocalizer<Properties.Resources> Localizer { get; set; } = default!;
}
