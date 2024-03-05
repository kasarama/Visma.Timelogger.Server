using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Visma.Timelogger.Application.Contracts;
using Visma.Timelogger.Application.VieModels;

namespace Visma.Timelogger.Application.Features.GetListProjectOverview
{
    public class GetListProjectOverviewQueryHandler : IRequestHandler<GetListProjectOverviewQuery, List<ProjectOverviewViewModel>>
    {
        private readonly ILogger<GetListProjectOverviewQueryHandler> _logger;
        private readonly AbstractValidator<GetListProjectOverviewQuery> _requestValidator;
        private readonly IApiRequestValidator _validator;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public GetListProjectOverviewQueryHandler(ILogger<GetListProjectOverviewQueryHandler> logger,
                                                  AbstractValidator<GetListProjectOverviewQuery> requestValidator,
                                                  IApiRequestValidator validator,
                                                  IProjectRepository projectRepository,
                                                  IMapper mapper)
        {
            _logger = logger;
            _requestValidator = requestValidator;
            _validator = validator;
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<List<ProjectOverviewViewModel>> Handle(GetListProjectOverviewQuery request, CancellationToken cancellationToken)
        {
            await _validator.ValidateRequest(request, _requestValidator, request.RequestId);

            var projects = await _projectRepository.GetListForFreelancerAsync(request.UserId);

            List<ProjectOverviewViewModel> result = _mapper.Map<List<ProjectOverviewViewModel>>(projects);

            return result;
        }
    }
}
