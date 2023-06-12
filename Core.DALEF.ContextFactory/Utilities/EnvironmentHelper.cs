namespace Core.DALEF.ContextFactory;

internal static class EnvironmentHelper
{
    public static bool IsProduction() => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToUpper() == "PRODUCTION";
    public static bool IsStaging() => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToUpper() == "STAGING";
    public static bool IsDevelopment() => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToUpper() == "DEVELOPMENT";
}
