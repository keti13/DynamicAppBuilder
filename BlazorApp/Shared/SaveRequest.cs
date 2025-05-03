namespace BlazorApp.Shared
{
    public class SaveRequest
    {
        public string Name { get; set; } = default!;
        public List<ControlType> Controls { get; set; } = new();
    }
}
