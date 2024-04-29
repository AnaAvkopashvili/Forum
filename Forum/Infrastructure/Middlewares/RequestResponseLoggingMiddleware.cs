namespace Forum.API.Infrastructure.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context.Request);
            await LogResponse(context.Response);
            await _next(context);
        }

        private async Task LogRequest(HttpRequest request)
        {
            _logger.LogInformation($"Logging request from middleware");

            var toLog = $"{Environment.NewLine} logged from middleware {Environment.NewLine}" +
                 $"IP: {request.HttpContext.Connection.RemoteIpAddress}{Environment.NewLine}" +
                 $"Address {request.Scheme}{Environment.NewLine}" +
                 $"Method {request.Method}{Environment.NewLine}" +
                 $"Path {request.Path}{Environment.NewLine}" +
                 $"IsSeccured {request.IsHttps}{Environment.NewLine}" +
                 $"QueryString {request.QueryString}{Environment.NewLine}" +
                 $"Time {DateTime.UtcNow}{Environment.NewLine}";

            _logger.LogInformation(toLog);
        }

        private async Task LogResponse(HttpResponse response)
        {
            _logger.LogInformation($"Logging response from middleware");

            var toLog = $"{Environment.NewLine} logged from middleware {Environment.NewLine}" +
                $"IP: {response.HttpContext.Connection.RemoteIpAddress}{Environment.NewLine}" +
                $"Body {response.Body}{Environment.NewLine}" +
                $"Status {response.StatusCode}{Environment.NewLine}" +
                 $"Time {DateTime.UtcNow}{Environment.NewLine}";

            _logger.LogInformation(toLog);
        }
    }
}
