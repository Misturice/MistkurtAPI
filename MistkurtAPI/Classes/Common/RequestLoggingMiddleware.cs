using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
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


            string request = await FormatRequest(context.Request);

            string response = await FormatResponse(context);


            _logger.LogInformation(
                "Method: {method} | Request: {request} | Response: {response}",
                context.Request?.Method,
                request,
                response);
            



        }


        private async Task<string> FormatRequest(HttpRequest request)
        {
            //Stream body = request.Body;

            request.EnableBuffering();

            byte[] buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            string bodyAsText = Encoding.UTF8.GetString(buffer);

            request.Body.Position = 0;

            return $"{request.Scheme} | {request.Host}{request.Path} | {request.QueryString} | {bodyAsText}";
        }

        private async Task<string> FormatResponse(HttpContext context)
        {
            Stream originalBodyStream = context.Response.Body;

            using(MemoryStream responseBody = new())
            {
                context.Response.Body = responseBody;

                await _next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                string responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
                return responseText;
            }
        }

    }
}
