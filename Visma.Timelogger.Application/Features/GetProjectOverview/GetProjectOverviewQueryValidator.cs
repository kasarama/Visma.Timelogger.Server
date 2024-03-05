
using FluentValidation;

namespace Visma.Timelogger.Application.Features.GetProjectOverview
{
    public class GetProjectOverviewQueryValidator : AbstractValidator<GetProjectOverviewQuery>
    {
        public GetProjectOverviewQueryValidator()
        {
            var required = "{PropertyName} is required.";

            RuleFor(c => c.UserId).NotEmpty().WithMessage("Freelancer Id is required.");
            RuleFor(c => c.ProjectId).NotEmpty().WithMessage(required);
        }
    }
}
