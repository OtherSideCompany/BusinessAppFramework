using MudBlazor;

namespace BusinessAppFramework.WebUI.Helpers
{
    public static class ColorCssHelper
    {
        public static string ToCssVariable(Color? color)
        {
            return color switch
            {
                Color.Primary => "var(--mud-palette-primary)",
                Color.Secondary => "var(--mud-palette-secondary)",
                Color.Success => "var(--mud-palette-success)",
                Color.Error => "var(--mud-palette-error)",
                Color.Warning => "var(--mud-palette-warning)",
                Color.Info => "var(--mud-palette-info)",
                _ => "var(--mud-palette-text-disabled)"
            };
        }
    }
}
