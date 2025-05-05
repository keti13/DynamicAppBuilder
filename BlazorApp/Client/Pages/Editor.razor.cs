using BlazorApp.Client.Services;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorApp.Client.Pages
{
    public partial class Editor : ComponentBase
    {
        [Inject] protected NavigationManager? NavigationManager { get; set; }
        [Inject] private CanvasStateService CanvasState { get; set; } = default!;
        [Inject] protected HttpClient Http { get; set; } = default!;
        [Inject] protected IJSRuntime JS { get; set; } = default!;
        [Inject] CanvasPreviewState? PreviewState { get; set; }

        protected List<ControlType> availableControls = new List<ControlType>
        {
            new() { Type = "Checkbox", Label = "Checkbox", IconUrl = "images/Checkbox.png" },
            new() { Type = "TextInput", Label = "Text Input", IconUrl = "images/TextInput.png", Placeholder = "type text", DefaultValue = "" },
            new() { Type = "NumberInput", Label = "Number Input", IconUrl = "images/NumberInput.png", Placeholder = "0", DefaultValue = "" },
            new() { Type = "Dropdown", Label = "Dropdown", IconUrl = "images/DropDown.png", Placeholder = "select an option" },
            new() { Type = "DatetimePicker", Label = "Datetime Picker", IconUrl = "images/DateTimePicker.png" },
            new() { Type = "Button", Label = "Button", IconUrl = "images/Button.png", DefaultValue = "Button" }
        };

        private string selectedView = "Desktop";
        private bool isViewManuallySelected = false; 
        private bool CanSave => canvasControls.Any();

        protected List<ControlType> canvasControls = new();

        protected ControlType? selectedControl;

        private bool showLoadModal = false;
        private List<ScreenSummary> savedScreens = new();

        private string? saveStatus;
        private string? saveStatusClass;
        private bool isSaving = false;
        private bool isLoading = false;

        private void SetView(string view)
        {
            selectedView = view;
            CanvasState.SelectedView = view;
            CanvasState.IsViewManuallySelected = true;
            isViewManuallySelected = true;
        }

        private async Task LoadScreen(int screenId)
        {
            try
            {
                var screen = await Http.GetFromJsonAsync<ScreenDefinition>($"api/screendefinitions/load/{screenId}");

                if (screen != null && !string.IsNullOrWhiteSpace(screen.JsonDefinition))
                {
                    var controls = JsonSerializer.Deserialize<List<ControlType>>(screen.JsonDefinition);

                    if (controls != null)
                    {
                        canvasControls = controls;
                        selectedControl = null;
                        showLoadModal = false;
                        StateHasChanged();
                        await JS.InvokeVoidAsync("enableDrag", ".draggable");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading screen: {ex.Message}");
            }
        }


        protected void HandleControlSelected(ControlType control)
        {
            selectedControl = control;
        }

        protected async void HandleSave()
        {
            var screenName = await JS.InvokeAsync<string>("prompt", "Enter a name for the screen:");

            if (string.IsNullOrWhiteSpace(screenName))
            {
                return;
            }

            var saveRequest = new SaveRequest
            {
                Name = screenName,
                Controls = canvasControls
            };

            isSaving = true;
            StateHasChanged();

            try
            {
                var response = await Http.PostAsJsonAsync("api/screendefinitions/save", saveRequest);

                if (response.IsSuccessStatusCode)
                {
                    saveStatus = "✅ Screen saved successfully!";
                    saveStatusClass = "alert alert-success";
                }
                else
                {
                    saveStatus = "❌ Failed to save screen.";
                    saveStatusClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                saveStatus = $"❌ Error: {ex.Message}";
                saveStatusClass = "alert alert-danger";
            }

            isSaving = false;
            StateHasChanged();

            await Task.Delay(2000);
            saveStatus = null;
            saveStatusClass = null;
            StateHasChanged();
        }

        protected async Task HandleLoad()
        {
            isLoading = true;
            StateHasChanged();
            savedScreens = await Http.GetFromJsonAsync<List<ScreenSummary>>("api/screendefinitions/all");

            showLoadModal = true;
            isLoading = false;
            StateHasChanged();
        }

        private async Task HandleRun()
        {
            var size = await JS.InvokeAsync<BlazorApp.Shared.Size>("getElementSize", "canvas-dropzone");
            CanvasState.Controls = canvasControls;
            CanvasState.SelectedView = selectedView;
            CanvasState.IsViewManuallySelected = isViewManuallySelected;
            PreviewState.CanvasWidth = size.width;
            PreviewState.CanvasHeight = size.height;

            NavigationManager?.NavigateTo("/preview");
        }
    }
}
