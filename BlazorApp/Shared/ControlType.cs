namespace BlazorApp.Shared
{
    public class ControlType
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Type { get; set; }
        public string? Label { get; set; } = "Label";
        public string? Placeholder { get; set; } = "";
        public string? DefaultValue { get; set; } = "";
        public bool IsRequired { get; set; } = false;
        public string? CssClass { get; set; } = "";
        public string? IconUrl { get; set; }
        public int X { get; set; } = 10;
        public int Y { get; set; } = 10;

    }
}
