namespace IPSTemplate.AppServer.Filters.ExceptionLogging
{
    public class ExceptionLoggingFilterOptions
    {
        public ExceptionLoggingFilterOptions()
        {
            ExceptionMapper = new(this);

            Map<Exception>(LogLevel.Error);
            Map<ArgumentException>(LogLevel.Warning);
            Map<KeyNotFoundException>(LogLevel.Warning);
        }

        internal ExceptionMapper ExceptionMapper { get; init; }

        public bool PrintEndpoint { get; set; } = true;

        public bool PrintRequestDetails { get; set; } = true;

        public Func<Exception, Exception> ExceptionTransformation { get; set; }

        public ExceptionLoggingFilterOptions Map<T>(LogLevel logLevel) where T : Exception
        {
            ExceptionMapper.Map<T>(logLevel);
            return this;
        }

        public ExceptionLoggingFilterOptions Map<T>(Action<HttpContext, T, ILogger> configure) where T : Exception
        {
            ExceptionMapper.Map(configure);
            return this;
        }
    }
}
