namespace IPSTemplate.AppServer.Helpers;

public static class EnvironmentHelper
{
    public static bool IsDevelopment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
    public static bool IsStaging => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Staging";
    public static bool IsProduction => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";
    public static string GetConnectionString(string name) => Environment.ExpandEnvironmentVariables(Startup.Configuration.GetConnectionString(name));
    public static string Get(string name) => Environment.ExpandEnvironmentVariables(Startup.Configuration[name]);
    public static string TrimUrl(string url) => url.Trim(' ', '/');
    public static string JoinPath(this string src, string path) => $"{TrimUrl(src)}/{TrimUrl(path)}";
    public static int GetMaxDegreeOfParallelism(int numberOfParallelTasks) => Convert.ToInt32(Math.Ceiling(Environment.ProcessorCount * 0.8 * 2.0 / numberOfParallelTasks));
}
