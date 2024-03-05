using FluentValidation;

namespace Visma.Timelogger.Application.Contracts
{
    public interface IApiRequestValidator
    {
        public Task<bool> ValidateRequest<TR>(TR request, AbstractValidator<TR> validator, Guid requestId);
    }
}
