using IPSTemplate.AppServer.Helpers;

namespace IPSTemplate.AppServer.Filters.ExceptionLogging
{
    internal class ExceptionLogger
    {
        private readonly ILogger _logger;
        private readonly HttpContext _httpContext;
        private readonly ExceptionLoggingFilterOptions _options;

        internal ExceptionLogger(ILogger logger, HttpContext context, ExceptionLoggingFilterOptions options)
        {
            _logger = logger;
            _httpContext = context;
            _options = options;
        }

        internal void Log(LogLevel level, Exception exception)
        {
            var loggerScope = _options.PrintRequestDetails
                ? _logger.BeginScope(new Dictionary<string, object>
                {
                    { "RequestDetails", RequestDetails.FromHttpRequest(_httpContext.Request) }
                })
                : null;

            try
            {
                if (_options.PrintEndpoint)
                {
                    string endpoint = _httpContext.Request.ToShortDescriptionString();
                    _logger.Log(level, exception, "An exception occurred while executing the request at the endpoint {Endpoint}", endpoint);
                }
                else
                {
                    _logger.Log(level, exception, "An exception occurred while executing the request.");
                }
            }
            finally
            {
                if (loggerScope is not null)
                {
                    loggerScope.Dispose();
                }
            }
        }
    }

    internal class RequestDetails
    {
        public string ContentType { get; set; }
        public long? ContentLength { get; set; }
        public string Protocol { get; set; }
        public string QueryString { get; set; }
        public string Host { get; set; }
        public string Scheme { get; set; }
        public string Method { get; set; }

        public static RequestDetails FromHttpRequest(HttpRequest httpRequest) => new()
        {
            ContentType = httpRequest.ContentType,
            ContentLength = httpRequest.ContentLength,
            Protocol = httpRequest.Protocol,
            QueryString = httpRequest.QueryString.HasValue ? httpRequest.QueryString.Value : null,
            Host = httpRequest.Host.Value,
            Scheme = httpRequest.Scheme,
            Method = httpRequest.Method,
        };

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
