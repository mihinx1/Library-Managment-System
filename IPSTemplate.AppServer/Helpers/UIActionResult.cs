using IPSBlazor;
using IPSBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace IPSTemplate.AppServer.Helpers;

public class UIActionResult
{
    public string? Message { get; set; }

    public Color Status { get; set; }

    public RenderFragment AsAlert() => builder =>
    {
        builder.OpenComponent<IPSAlert>(0);
        builder.AddAttribute(1, nameof(IPSAlert.Color), Status);
        builder.AddAttribute(2, nameof(IPSAlert.ChildContent), (RenderFragment)((builder2) =>
        {
            builder2.AddContent(3, Message);
        }));
        builder.CloseComponent();
    };
}
