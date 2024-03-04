using FluentValidation;
using Microsoft.Extensions.Logging;
using Visma.Timelogger.Application.Contracts;
using Visma.Timelogger.Application.Exceptions;

namespace Visma.Timelogger.Application.Services
{
    public class RequestValidator : IRequestValidator
    {
        private ILogger<RequestValidator> _logger;

        public RequestValidator(ILogger<RequestValidator> logger)
        {
            _logger = logger;
        }

        public async Task<bool> ValidateRequest<TR>(TR request, AbstractValidator<TR> validator, Guid requestId)
        {
            var validationResults = await validator.ValidateAsync(request);

            if (validationResults.Errors.Count > 0)
            {
                _logger.LogError("RequestId: {id} - Invalid Request", requestId);
                throw new RequestValidationException(validationResults);
            }
            return true;
        }
    }
}
