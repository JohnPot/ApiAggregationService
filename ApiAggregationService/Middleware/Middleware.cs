using System.Net;
using System.Text.Json;

namespace ApiAggregationService.Middleware;

public class Middleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<Middleware> _logger;

    public Middleware(RequestDelegate next, ILogger<Middleware> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString(), "Unhandled exception occurred with response code {responseCode}", context.Response.StatusCode);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var response = new
        {
            error = "An unexpected error occurred",
            detail = ex.Message
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
