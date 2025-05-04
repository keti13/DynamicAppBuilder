using BlazorApp.Client.Services;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorApp.Client.Pages;
public partial class Preview : ComponentBase
{
    [Inject] protected NavigationManager Navigation { get; set; } = default!;
    [Inject] private CanvasStateService CanvasState { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;
    [Inject] private CanvasPreviewState PreviewState { get; set; } = default!;

    private List<ControlType> CanvasControls => CanvasState.Controls;
    private string ViewMode => CanvasState.SelectedView;

    private double canvasWidth => PreviewState.CanvasWidth;
    private double canvasHeight => PreviewState.CanvasHeight;

    private double previewWidth => canvasWidth * 2;
    private double previewHeight => canvasHeight * 1.3;

    private double ScaleX => previewWidth / canvasWidth;
    private double ScaleY => previewHeight / canvasHeight;

    private string AspectRatio => $"{previewWidth} / {previewHeight}";


}
