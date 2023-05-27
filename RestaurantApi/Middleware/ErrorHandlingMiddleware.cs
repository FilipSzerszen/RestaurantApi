﻿using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using RestaurantApi.Exceptions;

namespace RestaurantApi.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(NotFoundException notFoundException)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notFoundException.Message);
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);

                context.Response.StatusCode=500;
                await context.Response.WriteAsync("Something went wrong");
            };
        }
    }
}
