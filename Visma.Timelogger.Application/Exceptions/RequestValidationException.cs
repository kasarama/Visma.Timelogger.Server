using FluentValidation.Results;

namespace Visma.Timelogger.Application.Exceptions
{
    public class RequestValidationException : Exception
    {
        public List<string> ValidationErrors { get; set; }

        public RequestValidationException(ValidationResult validationResult)
        {
            ValidationErrors = new List<string>();

            foreach (var validationError in validationResult.Errors)
            {
                ValidationErrors.Add(validationError.ErrorMessage);
            }
        }
    }
}
