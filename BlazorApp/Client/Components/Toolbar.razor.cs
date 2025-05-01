using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Components
{
    public partial class Toolbar : ComponentBase
    {
        [Parameter] public EventCallback OnRun { get; set; }
        [Parameter] public EventCallback OnSave { get; set; }
        [Parameter] public EventCallback OnLoad { get; set; }

        protected async Task OnRunClicked() => await OnRun.InvokeAsync();
        protected async Task OnSaveClicked() => await OnSave.InvokeAsync();
        protected async Task OnLoadClicked() => await OnLoad.InvokeAsync();
    }
}
