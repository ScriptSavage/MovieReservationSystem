using Application.Exceptions;

namespace Api.Middlewares;

public class GlobalErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (DoesNotExistsException e)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
        }
        catch (InvalidLoginException e)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        catch (Exception e)
        {
            Console.WriteLine("Something went wrong");
            context.Response.StatusCode = 500;
        }
    }
}