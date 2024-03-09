using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using wg.shared.abstractions.Exceptions;
using wg.shared.infrastructure.Exceptions.DTOs;

namespace wg.shared.infrastructure.Exceptions;

internal sealed class ExceptionMiddleware(
    ILogger<ExceptionMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            await HandleErrorAsync(context, ex);
        }
    }

    private async Task HandleErrorAsync(HttpContext context, Exception exception)
    {
        var (statusCode, error) = exception switch
        {
            WgAuthException => (StatusCodes.Status400BadRequest, new ErrorDto(
                    GetException(nameof(WgAuthException)), "Wrong credentials")),
            WgException => (StatusCodes.Status400BadRequest, new ErrorDto( 
                GetException(exception.GetType().Name),exception.Message)),
            _ => (StatusCodes.Status500InternalServerError, new ErrorDto("server_error", 
                "There was an error"))
        };
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(error);
    }
    
    private string GetException(string name)
        => name.Underscore().ToLower().Replace("_exception", "");
}