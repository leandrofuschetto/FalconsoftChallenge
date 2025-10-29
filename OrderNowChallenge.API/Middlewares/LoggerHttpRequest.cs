namespace OrderNowChallenge.API.Middlewares
{
    public class LoggerHttpRequest
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggerHttpRequest> _logger;

        public LoggerHttpRequest(RequestDelegate next, ILogger<LoggerHttpRequest> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation($"REQUEST TRACE ID: {context.TraceIdentifier}");

            await _next(context);
        }
    }
}
