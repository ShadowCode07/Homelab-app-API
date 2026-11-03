using HomelabAPI.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace HomelabAPI.Presentation.ExceptionHandling
{
    // Registered as a SINGLETON by AddExceptionHandler<T>() -> only inject singleton-safe services (like ILogger).
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            ProblemDetails problem;

            if (exception is AppException appException)
            {
                // One of OURS: thrown on purpose, message is safe to show.
                problem = BuildProblem(appException);
            }
            else
            {
                // NOT ours: a bug or infrastructure failure. Log everything, show nothing.
                _logger.LogError(exception, "Unhandled exception for {Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);

                problem = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = exception.GetType().Name,
                    Detail = exception.Message
                };
            }

            problem.Instance = httpContext.Request.Path;

            httpContext.Response.StatusCode = problem.Status!.Value;

            // problem.GetType() so a ValidationProblemDetails keeps its "errors" dictionary in the JSON.
            await httpContext.Response.WriteAsJsonAsync(problem, problem.GetType(), options: null, contentType: "application/problem+json", cancellationToken);

            return true; // true = "handled, stop looking for other handlers"
        }

        private static ProblemDetails BuildProblem(AppException exception)
        {
            int status = MapStatusCode(exception.Category);

            // Declared once as the BASE type, so it can hold either kind of problem.
            ProblemDetails problem;

            if (exception.Field != null)
            {
                // Field-level error -> "errors": { "<Field>": [ "<Message>" ] } so the form can show it under the input.
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
                // No field -> plain problem, the message goes in "detail".
                problem = new ProblemDetails
                {
                    Title = exception.Title ?? "A request error occurred.",
                    Detail = exception.Message
                };
            }

            // Shared by both branches.
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
