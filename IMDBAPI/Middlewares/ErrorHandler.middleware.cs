using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using IMDBAPI.Services.CustomException;
using Microsoft.AspNetCore.Http;


namespace IMDBAPI.Middlewares
{
    public class ErrorHandler : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context,RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { Message = ex.Message, Name = ex.GetType().Name });
            }
            catch (InvalidOperationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { Message = ex.Message,Name=ex.GetType().Name });
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(new { Message = ex.Message,Name=ex.GetType().Name });
            }
            catch (ArgumentException ex) {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { Message = ex.Message,Name=ex.GetType().Name });
            }
            catch(ValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { Message = ex.Message,Name=ex.GetType().Name });
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { Message = ex.Message,Name=ex.GetType().Name });
            }
        }
    }
}
