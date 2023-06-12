using System.Runtime.InteropServices;

namespace IPSTemplate.UI.Blazor.Helpers;

public class RuntimeLocation
{
    public static bool IsClientSide => RuntimeInformation.OSDescription == "Browser"; // WASM
    public static bool IsServerSide => RuntimeInformation.OSDescription != "Browser";
}
