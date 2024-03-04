namespace Visma.Timelogger.Api.Middleware
{
    public class ErrorDto
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public ErrorDto(string message, int statusCode)
        {
            this.Message = message;
            this.StatusCode = statusCode;
        }
    }
}
