using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Components
{
    public partial class PropertiesPanel : ComponentBase
    {
        [Parameter] public ControlType SelectedControl { get; set; }

    }
}
