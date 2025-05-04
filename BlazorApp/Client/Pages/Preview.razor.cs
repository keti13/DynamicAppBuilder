using BlazorApp.Client.Services;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Drawing;

namespace BlazorApp.Client.Pages;
public partial class Preview : ComponentBase
{
    [Inject] protected NavigationManager Navigation { get; set; } = default!;
    [Inject] private CanvasStateService CanvasState { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;
    private List<ControlType> CanvasControls => CanvasState.Controls;
    private string ViewMode => CanvasState.SelectedView;

    private double canvasWidth = 380;  // This should match the editor canvas size
    private double canvasHeight = 430;

    private double previewWidth = 1;
    private double previewHeight = 1;

    private double ScaleX => previewWidth / canvasWidth;
    private double ScaleY => previewHeight / canvasHeight;

    private string AspectRatio => $"{canvasWidth} / {canvasHeight}";


    private class Size
    {
        public double width { get; set; }
        public double height { get; set; }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JS.InvokeVoidAsync("scaleCanvasToFit");
        }
    }

}
