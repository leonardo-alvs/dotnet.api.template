using Serilog;
using System.Net;
using System.Reflection;
using System.Text.Json;

namespace Template.Api.Middlewares
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {

        public GlobalExceptionHandlerMiddleware()
        {

        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (InvalidOperationException ex)
            {
                await HandleBadRequestAsync(context, ex);
            }
            catch (AggregateException ex)
            {
                await HandleExceptionAsync(context, ex.InnerExceptions.First());
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            Log.Error(exception, $"Error on {Assembly.GetExecutingAssembly().GetName().Name}");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(JsonSerializer.Serialize(new { Message = "Houve um erro ao processar sua requisição" }));
        }

        private Task HandleBadRequestAsync(HttpContext context, InvalidOperationException ex)
        {
            Log.Warning($"Warning: {ex.Message}");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return context.Response.WriteAsync(JsonSerializer.Serialize(new { ex.Message }));
        }
    }
}
