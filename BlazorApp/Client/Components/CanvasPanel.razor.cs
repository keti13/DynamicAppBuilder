using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Text.Json;
using BlazorApp.Shared;

namespace BlazorApp.Client.Components
{
    public partial class CanvasPanel : ComponentBase
    {
        [Parameter]
        public List<ControlType> CanvasControls { get; set; } = new List<ControlType>();

        [Parameter]
        public string ViewMode { get; set; } = "Desktop";

        [Parameter]
        public EventCallback<ControlType> OnSelectControl { get; set; }

        [Parameter]
        public EventCallback<ControlType> OnControlDropped { get; set; }

        [Inject]
        protected IJSRuntime JS { get; set; } = default!;

        protected async Task HandleDrop(DragEventArgs e)
        {
            try
            {
                // Always use the fully qualified function name to avoid conflicts
                var controlType = await JS.InvokeAsync<string>("window.getDragData", e);
                Console.WriteLine($"Drop detected for control: {controlType}");

                if (!string.IsNullOrEmpty(controlType))
                {
                    // Get the position where the control was dropped (use fallback if event doesn't have coordinates)
                    var positionJson = await JS.InvokeAsync<string>(
                        "window.getDropPosition", e, "canvas-dropzone");

                    var position = JsonSerializer.Deserialize<Position>(positionJson)
                        ?? new Position { X = 10, Y = 10 };

                    // Create the new control with the correct position
                    var control = new ControlType
                    {
                        Type = controlType,
                        Label = controlType,
                        // Use the position data from JS
                        X = position.X,
                        Y = position.Y
                    };

                    // Add it to the canvas
                    CanvasControls.Add(control);

                    // Notify parent component
                    await OnControlDropped.InvokeAsync(control);

                    Console.WriteLine($"Added control at position: X={position.X}, Y={position.Y}");
                }
                else
                {
                    Console.WriteLine("Error: No control type data received from the drag operation");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in HandleDrop: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        protected void HandleControlSelect(ControlType control)
        {
            OnSelectControl.InvokeAsync(control);
        }

        // Helper class for deserializing position data
        private class Position
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}