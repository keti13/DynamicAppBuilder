using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorApp.Client.Components
{
    public partial class ComponentsPanel : ComponentBase
    {
        [Inject]
        private IJSRuntime JS { get; set; } = default!;

        [Parameter] public List<ControlType> AvailableControls { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JS.InvokeVoidAsync("setupComponentDragToCanvas");
            }
        }
    }
}
