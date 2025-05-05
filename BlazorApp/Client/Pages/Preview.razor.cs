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

    private DotNetObjectReference<Preview>? dotNetRef { get; set; } = default!;
    private double canvasWidth => PreviewState.CanvasWidth;
    private double canvasHeight => PreviewState.CanvasHeight;

    private double previewWidth => canvasWidth * 1.5;
    private double previewHeight => canvasHeight * 1.5;

    private double ScaleX => previewWidth / canvasWidth;
    private double ScaleY => previewHeight / canvasHeight;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            dotNetRef = DotNetObjectReference.Create(this);
            await JS.InvokeVoidAsync("registerResizeHandler", dotNetRef);

            var width = await JS.InvokeAsync<int>("getViewportWidth");
            UpdateViewBasedOnWidth(width);
        }
    }

    [JSInvokable]
    public void OnViewportResize(int width)
    {
        UpdateViewBasedOnWidth(width);
    }

    private void UpdateViewBasedOnWidth(int width)
    {
        if (CanvasState.IsViewManuallySelected)
            return;

        if (width < 768 && CanvasState.SelectedView != "Mobile")
        {
            CanvasState.SelectedView = "Mobile";
            StateHasChanged();
        }
        else if (width >= 768 && CanvasState.SelectedView != "Desktop")
        {
            CanvasState.SelectedView = "Desktop";
            StateHasChanged();
        }
    }
}
