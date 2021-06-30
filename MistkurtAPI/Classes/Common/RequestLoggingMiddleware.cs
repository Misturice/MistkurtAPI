using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MistkurtAPI.Classes.Common
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            finally
            {
                _logger.LogInformation(
                    "Request {method} | {url} | {body} => {statusCode} | {responseBody}",
                    context.Request?.Method,
                    context.Request?.Path.Value,
                    context.Request?.BodyReader,
                    context.Response?.StatusCode,
                    context.Response?.Body);
            }
        }

    }
}
