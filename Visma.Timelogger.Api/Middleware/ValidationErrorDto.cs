namespace Visma.Timelogger.Api.Middleware
{
    public class ValidationErrorDto : ErrorDto
    {
        public List<string> ErrorList { get; set; } = new List<string>();
        public ValidationErrorDto(string message, int statusCode, List<string> errors) : base(message, statusCode)
        {
            ErrorList = errors;
        }
        public ValidationErrorDto() : base("", 0) { }
    }
}
