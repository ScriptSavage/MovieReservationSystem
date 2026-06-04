using Application.Exceptions;
using FluentValidation;

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
            Console.WriteLine(e.Message);
        }
        catch (InvalidLoginException e)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        catch (ArgumentException e)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            Console.WriteLine(e.Message);
        }
        catch (ValidationException e)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            Console.WriteLine(e.Message);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (InvalidDateException e)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            Console.WriteLine(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("Something went wrong");
            Console.WriteLine(e);
            context.Response.StatusCode = 500;
        }
    }
}