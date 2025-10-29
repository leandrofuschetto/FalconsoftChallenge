using RecruitingChallenge.Domain.Exceptions;
using System.Net;
using System.Reflection;
using System.Text.Json;

namespace RecruitingChallenge.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(
            RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
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
                await HandleException(context, ex);
            }
        }

        private Task HandleException(HttpContext context, Exception error)
        {
            var customCode = GetCustomCode(error);

            _logger.LogInformation($"REQUEST TRACE ID: {context.TraceIdentifier}");

            var response = context.Response;
            response.ContentType = "application/json";

            switch (error)
            {
                case AuthenticationErrorException:
                case JWTException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case ArgumentException:
                case OrderCannotBeModifiedException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case UserNotFoundException:
                case OrderNotFoundException:
                case OrderItemNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case DataBaseContextException:
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer
                .Serialize(new { message = error?.Message, code = customCode });

            return response.WriteAsync(result);
        }

        private string GetCustomCode(Exception ex)
        {
            string code = "GENERAL_ERROR";

            var prop = ex.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.Name == "Code")
                .FirstOrDefault();

            if (prop != null)
                code = prop.GetValue(ex, null).ToString();

            return code;
        }
    }
}
