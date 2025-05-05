using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Components
{
    public partial class PreviewControlRenderer : ComponentBase
    {
        [Parameter] public ControlType Control { get; set; } = default!;
        [Parameter] public bool IsMobile { get; set; } = false;

        private string GetWrapperClass() => IsMobile ? "mobile-control-wrapper mb-3 mt-3" : "mb-2";
        private string GetInputClass() => IsMobile ? "form-control mobile-full" : "form-control";
        private string GetButtonClass() => IsMobile ? "mobile-full" : "w-auto btn";
    }
}
