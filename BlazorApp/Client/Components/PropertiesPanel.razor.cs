using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Components
{
    public partial class PropertiesPanel : ComponentBase
    {
        [Parameter] public ControlType? SelectedControl { get; set; }
        [Parameter] public EventCallback OnControlChanged { get; set; }

        protected override void OnParametersSet()
        {
            if (SelectedControl?.Type == "Dropdown")
            {
                while (SelectedControl.Options.Count < 3)
                {
                    SelectedControl.Options.Add(string.Empty);
                }
            }
        }

        private async Task NotifyChanged()
        {
            if (OnControlChanged.HasDelegate)
            {
                await OnControlChanged.InvokeAsync();
            }
        }

        private async Task OnDatetimeChanged(string value)
        {
            SelectedControl.DefaultValue = value;
            await NotifyChanged();
        }
        private void UpdateOption(int index, string value)
        {
            while (SelectedControl.Options.Count <= index)
            {
                SelectedControl.Options.Add(string.Empty);
            }

            SelectedControl.Options[index] = value;
        }
    }
}
