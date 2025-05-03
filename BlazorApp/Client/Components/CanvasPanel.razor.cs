using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorApp.Client.Components
{
    public partial class CanvasPanel : ComponentBase
    {
        [Inject] protected IJSRuntime JS { get; set; } = default!;
        [Parameter] public List<ControlType> CanvasControls { get; set; } = new List<ControlType>();
        [Parameter]  public string ViewMode { get; set; } = "Desktop";
        [Parameter] public EventCallback<ControlType> OnSelectControl { get; set; }
        [Parameter] public ControlType? SelectedControl { get; set; }
        [Parameter] public List<ControlType> AvailableControls { get; set; } = new();

        private DotNetObjectReference<CanvasPanel>? _dotNetRef;

        protected override void OnInitialized()
        {
            _dotNetRef = DotNetObjectReference.Create(this);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _dotNetRef = DotNetObjectReference.Create(this);
                await JS.InvokeVoidAsync("setupComponentDragToCanvas", _dotNetRef);
                await JS.InvokeVoidAsync("enableDrag", ".draggable");
                await JS.InvokeVoidAsync("registerBlazorCanvasRef", _dotNetRef); // NEW
            }
        }

        [JSInvokable]
        public async Task HandleDropFromJS(string controlType, double x, double y)
        {
            var template = AvailableControls.FirstOrDefault(c => c.Type == controlType);

            var control = new ControlType
            {
                Id = Guid.NewGuid(),
                Type = controlType,
                Label = template?.Label ?? controlType,
                Placeholder = template?.Placeholder,
                DefaultValue = template?.DefaultValue,
                X = (int)x,
                Y = (int)y
            };

            CanvasControls.Add(control);
            await JS.InvokeVoidAsync("enableDrag", ".draggable");
            StateHasChanged();
        }

        [JSInvokable]
        public void UpdateControlPosition(string id, double x, double y)
        {
            var control = CanvasControls.FirstOrDefault(c => c.Id.ToString() == id);
            if (control != null)
            {
                control.X = (int)x;
                control.Y = (int)y;
            }
        }

        protected void HandleControlSelect(ControlType control)
        {
            OnSelectControl.InvokeAsync(control);
        }

        private void RemoveControl(ControlType control)
        {
            CanvasControls.Remove(control);
            SelectedControl = null;
            OnSelectControl.InvokeAsync(null);
            StateHasChanged();
        }

        private void ClearSelection()
        {
            OnSelectControl.InvokeAsync(null);
        }

        public void Dispose()
        {
            _dotNetRef?.Dispose();
        }
    }
}