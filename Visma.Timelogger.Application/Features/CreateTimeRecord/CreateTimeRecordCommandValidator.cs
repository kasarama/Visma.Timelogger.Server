using FluentValidation;

namespace Visma.Timelogger.Application.Features.CreateTimeRecord
{
    public class CreateTimeRecordCommandValidator : AbstractValidator<CreateTimeRecordCommand>
    {
        public CreateTimeRecordCommandValidator()
        {
            var required = "{PropertyName} is required.";

            RuleFor(c => c.FreelancerId).NotEmpty().WithMessage(required);
            RuleFor(c => c.ProjectId).NotEmpty().WithMessage(required);
            RuleFor(c => c.StartTime).NotEmpty().WithMessage(required);
            RuleFor(c => c.DurationMinutes).NotEmpty().WithMessage(required)
                                           .GreaterThanOrEqualTo(30).WithMessage("Duration must be of at least 30 minutes.")
                                           .LessThanOrEqualTo(60 * 24).WithMessage("Duration must be of max 24 hours.");
        }
    }
}
