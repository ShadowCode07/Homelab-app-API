using HomelabAPI.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HomelabAPI.Presentation.ExceptionHandling
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IHostEnvironment _environment;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            ProblemDetails problem;

            if (exception is AppException appException)
            {
                problem = BuildProblem(appException);
            }
            else
            {
                _logger.LogError(exception, "Unhandled exception for {Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);

                problem = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "An unexpected error occurred."
                };

                if (_environment.IsDevelopment())
                {
                    problem.Detail = $"{exception.GetType().Name}: {exception.Message}";
                }
            }

            problem.Instance = httpContext.Request.Path;

            httpContext.Response.StatusCode = problem.Status!.Value;

            await httpContext.Response.WriteAsJsonAsync(problem, problem.GetType(), options: null, contentType: "application/problem+json", cancellationToken);

            return true;
        }

        private static ProblemDetails BuildProblem(AppException exception)
        {
            int status = MapStatusCode(exception.Category);

            ProblemDetails problem;

            if (exception.Field != null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    [exception.Field] = new[] { exception.Message }
                };

                problem = new ValidationProblemDetails(errors)
                {
                    Title = exception.Title ?? "One or more validation errors occurred."
                };
            }
            else
            {
                problem = new ProblemDetails
                {
                    Title = exception.Title ?? "A request error occurred.",
                    Detail = exception.Message
                };
            }

            problem.Status = status;
            problem.Extensions["errorCode"] = exception.ErrorCode;

            return problem;
        }

        private static int MapStatusCode(ErrorCategory category)
        {
            return category switch
            {
                ErrorCategory.Validation => StatusCodes.Status400BadRequest,
                ErrorCategory.Conflict => StatusCodes.Status409Conflict,
                ErrorCategory.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
        }
    }
}
