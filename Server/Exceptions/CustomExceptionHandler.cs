using Microsoft.AspNetCore.Diagnostics;

namespace Server.Exceptions;

public class CustomExceptionHandler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is NotFoundException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return new ValueTask<bool>(true);
        }

        if (exception is ForbiddenException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            return new ValueTask<bool>(true);
        }
        
        if (exception is BadRequestException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return new ValueTask<bool>(true);
        }

        if (exception is UnauthorizedException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return new ValueTask<bool>(true);
        }

        if (exception is ConflictException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
            return new ValueTask<bool>(true);
        }

        return ValueTask.FromResult(false);
    }
}