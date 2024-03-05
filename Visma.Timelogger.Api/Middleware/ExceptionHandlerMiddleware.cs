using System.Net;
using System.Text.Json;
using Visma.Timelogger.Application.Exceptions;

namespace Visma.Timelogger.Api.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<ExceptionHandlerMiddleware> _logger { get; }

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ConvertException(context, ex);
            }
        }

        private Task ConvertException(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var result = string.Empty;
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;


            switch (exception)
            {
                case RequestValidationException validationException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new ValidationErrorDto("Invalid request", 400, validationException.ValidationErrors));
                    break;
                case BadRequestException badRequestException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new ErrorDto(badRequestException.Message, 400));
                    break;
                default:
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    result = JsonSerializer.Serialize(new ErrorDto("Internal server error", 500));
                    break;
            }

            _logger.LogInformation("Exception Handled in ExceptionHandlerMiddleware");
            context.Response.StatusCode = (int)httpStatusCode;

            return context.Response.WriteAsync(result);
        }
    }
}
