namespace IPSTemplate.AppServer.Middleware
{
    public static class OptionsVerbMiddlewareExtensions
    {
        public static IApplicationBuilder UseOptionsVerbHandler(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OptionsVerbMiddleware>();
        }
    }
}
