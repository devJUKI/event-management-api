using System.Text.RegularExpressions;

namespace EventManagementAPI.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BadHttpRequestException ex)
        {
            _logger.LogError(ex, "Bad request error");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            string error = ex.Message.Replace("\"", "'");

            var errorResponse = new
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Bad Request",
                Status = 400,
                Errors = new List<string>() { error },
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Internal server error");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            string error = "An unexpected error occurred. Please try again later.";

            var errorResponse = new
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Internal Server Error",
                Status = 500,
                Errors = new List<string>() { error },
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
