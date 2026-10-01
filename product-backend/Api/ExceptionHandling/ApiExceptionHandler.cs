using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Polly.Timeout;
using System.Text.Json;

namespace Api.ExceptionHandling
{
    public sealed class ApiExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ApiExceptionHandler> _logger;
        private readonly IProblemDetailsService _problemDetailsService;

        public ApiExceptionHandler(
            ILogger<ApiExceptionHandler> logger,
            IProblemDetailsService problemDetailsService)
        {
            _logger = logger;
            _problemDetailsService = problemDetailsService;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception,
                "Unhandled exception processing {Method} {Path}",
                httpContext.Request.Method,
                httpContext.Request.Path);

            var problemDetails = exception switch
            {
                ArgumentOutOfRangeException =>
                    new ProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Invalid request."
                    },

                KeyNotFoundException =>
                    new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Product not found.",
                        Detail = exception.Message
                    },

                TimeoutRejectedException or TaskCanceledException =>
                    new ProblemDetails
                    {
                        Status = StatusCodes.Status504GatewayTimeout,
                        Title = "External service timed out.",
                        Detail =
                            "The product service did not respond in time."
                    },

                HttpRequestException or JsonException =>
                    new ProblemDetails
                    {
                        Status = StatusCodes.Status502BadGateway,
                        Title = "External service unavailable.",
                        Detail =
                            exception is JsonException
                                ? "The product service returned an invalid response."
                                : "The product service could not be reached."
                    },

                _ =>
                    new ProblemDetails
                    {
                        Status =
                            StatusCodes.Status500InternalServerError,
                        Title = "An unexpected error occurred.",
                        Detail =
                            "The request could not be completed."
                    }
            };

            httpContext.Response.StatusCode =
                problemDetails.Status!.Value;

            await _problemDetailsService.WriteAsync(
                new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    ProblemDetails = problemDetails,
                    Exception = exception
                });

            return true;
        }
    }
}
