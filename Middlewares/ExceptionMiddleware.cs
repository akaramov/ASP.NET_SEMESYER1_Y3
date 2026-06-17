using APAERMENT_LAST_API.Exceptions;
using APARTMENT_API.Exceptions;
using System.Net;
using System.Text.Json;

namespace APAERMENT_LAST_API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _request;

        public ExceptionMiddleware(RequestDelegate request)
        {
            _request = request;
        }

        private static async Task HandleException(
            HttpContext context,
            HttpStatusCode statusCode,
            string message
        )
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            var response = new
            {
                Status = false,
                StatusCode = statusCode,
                Message = message,
                Data = new { }
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _request(context);
            }
            catch (BadRequestException ex)
            {
                await HandleException(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (UnauthorizeException ex)
            {
                await HandleException(context, HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (NotFoundException ex)
            {
                await HandleException(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (ValidationException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 400;
                var response = new
                {
                    Success = false,
                    StatusCode = 400,
                    Message = ex.Message,
                    Errors = ex.Errors
                };
                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
            catch (Exception ex)
            {
                await HandleException(context, HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
