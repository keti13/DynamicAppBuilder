using BlazorApp.Client.Services;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorApp.Client.Pages;
public partial class Preview : ComponentBase
{
    [Inject] protected NavigationManager Navigation { get; set; } = default!;
    [Inject] private CanvasStateService CanvasState { get; set; } = default!;

    private List<ControlType> CanvasControls => CanvasState.Controls;
    private string ViewMode => CanvasState.SelectedView;
}
