using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Visma.Timelogger.Application.Contracts;
using Visma.Timelogger.Application.Exceptions;
using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Application.Features.CreateTimeRecord
{
    public class CreateTimeRecordCommandHandler : IRequestHandler<CreateTimeRecordCommand, bool>
    {
        private readonly ILogger<CreateTimeRecordCommandHandler> _logger;
        private readonly IRequestValidator _validator;
        private readonly IAsyncRepository<Project> _projectRepository;
        private readonly IAsyncRepository<TimeRecord> _timeRcordRepository;
        private readonly IMapper _mapper;


        public CreateTimeRecordCommandHandler(ILogger<CreateTimeRecordCommandHandler> logger,
                                              IRequestValidator validator,
                                              IAsyncRepository<Project> projectRepository,
                                              IAsyncRepository<TimeRecord> timeRecordRepository,
                                              IMapper mapper)
        {
            _logger = logger;
            _validator = validator;
            _projectRepository = projectRepository;
            _timeRcordRepository = timeRecordRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateTimeRecordCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateTimeRecordCommandValidator();
            await _validator.ValidateRequest(request, validator, request.RequestId);

            var project = await ProjectExists(request.ProjectId, request.RequestId);

            if (project.IsCompleted) {
                _logger.LogWarning("RequestId: {id} - Bad request: Attempt to register TimeRecord: (start: {start}, duaration: {duration}) on a completed Project {projectId}.", 
                                    request.RequestId, project.Id, request.StartTime, request.DurationMinutes);
                throw new BadRequestException("Project completed Time registration is outside the project time period");
            }

            TimeRecord timeRecord = _mapper.Map<TimeRecord>(request);

            IsTimeRecordWithinProjectPeriod(project, timeRecord, request.RequestId);

      

            await _timeRcordRepository.AddAsync(timeRecord);

            return true;
        }

        public bool IsTimeRecordWithinProjectPeriod(Project project, TimeRecord timeRecord, Guid requestId)
        {
            if (timeRecord.StartTime >= project.StartTime && timeRecord.StartTime.AddMinutes(timeRecord.DurationMinutes) <= project.Deadline)
            {
                return true;
            }
            else
            {
                _logger.LogWarning("RequestId: {id} - Bad request: ProjectId {projectId} - Time registration is outside the project time period.", requestId, project.Id);
                throw new BadRequestException("Time registration is outside the project time period");
            }
        }

        public async Task<Project> ProjectExists(Guid projectId, Guid requestId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);

            if (project == null)
            {
                _logger.LogWarning("RequestId: {id} - Bad request: Project {projectId} does not exist.", requestId, projectId);
                throw new BadRequestException($"Project '{projectId}' does not exists.");
            }
            return project;
        }
    }
}
