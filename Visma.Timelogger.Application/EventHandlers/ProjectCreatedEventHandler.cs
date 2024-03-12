using AutoMapper;
using Microsoft.Extensions.Logging;
using Visma.Timelogger.Application.Contracts;
using Visma.Timelogger.Application.Events.Sub;
using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Application.EventHandlers
{
    public class ProjectCreatedEventHandler : IEventHandler<ProjectCreatedEvent>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ILogger<ProjectCreatedEventHandler> _logger;
        private readonly IMapper _mapper;

        public ProjectCreatedEventHandler(IProjectRepository projectRepository,
                                  ILogger<ProjectCreatedEventHandler> logger,
                                  IMapper mapper)
        {
            _projectRepository = projectRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Handle(ProjectCreatedEvent @event)
        {
            Project project = _mapper.Map<Project>(@event);
            await _projectRepository.AddAsync(project);
        }
    }
}
