using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Pages
{
    public partial class Editor : ComponentBase
    {
        [Inject] protected NavigationManager NavigationManager { get; set; }

        protected List<ControlType> availableControls = new List<ControlType>
        {
            new() { Type = "Checkbox", Label = "Checkbox", IconUrl = "images/Checkbox.png" },
            new() { Type = "TextInput", Label = "Text Input", IconUrl = "images/TextInput.png" },
            new() { Type = "NumberInput", Label = "Number Input", IconUrl = "images/NumberInput.png" },
            new() { Type = "Dropdown", Label = "Dropdown", IconUrl = "images/DropDown.png" },
            new() { Type = "DatetimePicker", Label = "Datetime Picker", IconUrl = "images/DateTimePicker.png" },
            new() { Type = "Button", Label = "Button", IconUrl = "images/Button.png" }
        };
        private string selectedView = "Desktop";

        protected List<ControlType> canvasControls = new();

        protected ControlType selectedControl;

        protected void HandleControlDropped(ControlType control)
        {
            var copy = new ControlType
            {
                Type = control.Type,
                Label = control.Label,
                Placeholder = control.Placeholder,
                DefaultValue = control.DefaultValue
            };

            canvasControls.Add(copy);
        }

        protected void HandleControlSelected(ControlType control)
        {
            selectedControl = control;
        }

        protected void HandleSave()
        {
            // TODO: Save to backend
        }

        protected void HandleLoad()
        {
            // TODO: Load from backend
        }

        protected void HandleRun()
        {
            // TODO: Save first (optional), then navigate to preview
            NavigationManager.NavigateTo("/preview");
        }
    }
}
