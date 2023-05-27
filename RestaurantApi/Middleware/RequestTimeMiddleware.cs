using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RestaurantApi.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private Stopwatch _stopwatch;
        private readonly ILogger _logger;

        public RequestTimeMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
            _stopwatch = new Stopwatch();
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopwatch.Start();
            await next.Invoke(context);
            _stopwatch.Stop();

            var elapsedMiliSec = _stopwatch.ElapsedMilliseconds;
            var message = $"Request [{context.Request.Method}] at {context.Request.Path} took {elapsedMiliSec} ms";

            _logger.LogInformation(message);
        }
    }
}
