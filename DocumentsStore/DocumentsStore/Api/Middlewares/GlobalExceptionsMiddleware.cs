using System.Net;
using System.Text.Json;

namespace DocumentsStore.Api.Middlewares;

public class GlobalExceptionsMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionsMiddleware> _logger;

    private const string DefaultMessage = "Something went wrong. Please try again later";

    public GlobalExceptionsMiddleware(ILogger<GlobalExceptionsMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            _logger.LogError(ex, "Error processing request. Message: {message}", message);
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { Message = DefaultMessage }));
    }
}