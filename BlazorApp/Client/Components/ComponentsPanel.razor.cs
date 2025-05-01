using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Components
{
    public partial class ComponentsPanel : ComponentBase
    {
        [Parameter] public List<ControlType> AvailableControls { get; set; }
        [Parameter] public EventCallback<ControlType> OnControlDropped { get; set; }
    }
}
