using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Visma.Timelogger.Application.Contracts;
using Visma.Timelogger.Application.Exceptions;
using Visma.Timelogger.Application.VieModels;

namespace Visma.Timelogger.Application.Features.GetProjectOverview
{
    public class GetProjectOverviewQueryHandler : IRequestHandler<GetProjectOverviewQuery, ProjectOverviewViewModel>
    {
        private readonly ILogger<GetProjectOverviewQueryHandler> _logger;
        private readonly AbstractValidator<GetProjectOverviewQuery> _requestValidator;
        private readonly IApiRequestValidator _validator;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public GetProjectOverviewQueryHandler(ILogger<GetProjectOverviewQueryHandler> logger,
                                              AbstractValidator<GetProjectOverviewQuery> commandValidator,
                                              IApiRequestValidator validator,
                                              IProjectRepository projectRepository,
                                              IMapper mapper)
        {
            _logger = logger;
            _requestValidator = commandValidator;
            _validator = validator;
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<ProjectOverviewViewModel> Handle(GetProjectOverviewQuery request, CancellationToken cancellationToken)
        {
            await _validator.ValidateRequest(request, _requestValidator, request.RequestId);

            var project = await _projectRepository.GetByIdForFreelancerAsync(request.ProjectId, request.UserId);

            if (project == null)
            {
                _logger.LogWarning("RequestId: {id} - Bad request: Cannot find Project {projectId}.", request.RequestId, request.ProjectId);
                throw new BadRequestException($"Cannot find Project {request.ProjectId} for Freelancer {request.UserId}.");
            }

            ProjectOverviewViewModel result = _mapper.Map<ProjectOverviewViewModel>(project);
            return result;
        }
    }
}
