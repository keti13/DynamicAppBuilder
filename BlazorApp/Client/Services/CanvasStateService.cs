using BlazorApp.Shared;

namespace BlazorApp.Client.Services
{
    public class CanvasStateService
    {
        public List<ControlType> Controls { get; set; } = new();
        public string SelectedView { get; set; } = "Desktop";
        public bool IsViewManuallySelected { get; set; } = false;
    }
}
