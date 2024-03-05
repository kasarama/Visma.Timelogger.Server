using FluentValidation;

namespace Visma.Timelogger.Application.Features.GetListProjectOverview
{
    public class GetListProjectOverviewQueryValidator : AbstractValidator<GetListProjectOverviewQuery>
    {
        public GetListProjectOverviewQueryValidator()
        {
            RuleFor(c => c.UserId).NotEmpty().WithMessage("Freelancer Id is required.");
        }
    }
}
