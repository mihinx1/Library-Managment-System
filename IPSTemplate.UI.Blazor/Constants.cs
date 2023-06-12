using IPSBlazor.Models;

namespace IPSTemplate.UI.Blazor
{
    public static class Constants
    {
        public static readonly string AppVersion = "1.0.0";

        public static readonly CultureItem[] SupportedCultures = new[]
        {
            new CultureItem("en-US", "English"),
            new CultureItem("sl-SI", "Slovenščina"),
        };

        public static readonly CultureItem[] SupportedUICultures = new[]
        {
            new CultureItem("en-US", "English"),
            new CultureItem("sl-SI", "Slovenščina"),
        };

        public class LocalStorageKeys
        {
            public static readonly string DrawerMinimized = "drawerMinimized";
        }
    }
}
