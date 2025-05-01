using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorApp.Client.Components
{
    public partial class CanvasPanel : ComponentBase
    {
        [Parameter] public List<ControlType> CanvasControls { get; set; }
        [Parameter] public EventCallback<ControlType> OnSelectControl { get; set; }

        [Parameter] public string ViewMode { get; set; } = "Desktop";
        [Parameter] public EventCallback<ControlType> OnControlDropped { get; set; }

        private async Task HandleDrop(DragEventArgs e)
        {
            Console.WriteLine("Drop detected");

            var controlType = await JS.InvokeAsync<string>("getDragData", e);

            var newControl = new ControlType
            {
                Type = controlType,
                Label = controlType
            };

            await OnControlDropped.InvokeAsync(newControl);
        }

    }
}
