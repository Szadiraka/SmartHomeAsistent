using System.Net;
using System.Text.Json;
using SmartHomeAsistent.CustomExceptions;


namespace SmartHomeAsistent.middleware
{
    public class ErrorHadlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHadlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }


        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpResponse response = context.Response;

            response.ContentType = "application/json";
            int statusCode;
            switch (ex)
            {
                case BadRequestException:
                   statusCode =StatusCodes.Status400BadRequest;
                    break;
                case NotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    break;
                case UnauthorizedAccessException:
                    statusCode = StatusCodes.Status401Unauthorized;
                    break;
                case ValidationException:
                    statusCode = StatusCodes.Status400BadRequest;
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            response.StatusCode = statusCode;

            var result = JsonSerializer.Serialize(new 
            { 
                success = false,
                message = ex.Message
            });
            return response.WriteAsync(result); 

        }

    }
}
