using AutoMapper;
using FluentValidation;
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
        private readonly AbstractValidator<CreateTimeRecordCommand> _commandValidator;
        private readonly IRequestValidator _validator;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public CreateTimeRecordCommandHandler(ILogger<CreateTimeRecordCommandHandler> logger,
                                              AbstractValidator<CreateTimeRecordCommand> commandValidator,
                                              IRequestValidator validator,
                                              IProjectRepository projectRepository,
                                              IMapper mapper)
        {
            _logger = logger;
            _commandValidator = commandValidator;
            _validator = validator;
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateTimeRecordCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateRequest(request, _commandValidator, request.RequestId);

            var project = await ProjectExists(request);

            IsTimeRecordInPast(request);
            IsTimeRecordWithinProjectPeriod(project, request);

            TimeRecord timeRecord = _mapper.Map<TimeRecord>(request);
            project.TimeRecords.Add(timeRecord);

            await _projectRepository.AddTimeRecordAsync(project);

            return true;
        }

        public async Task<Project> ProjectExists(CreateTimeRecordCommand request)
        {
            var project = await _projectRepository.GetActiveByProjectIdForFreelancerAsync(request.ProjectId, request.FreelancerId);

            if (project == null)
            {
                _logger.LogWarning("RequestId: {id} - Bad request: Cannot register Time for Project {projectId}.", request.RequestId, request.ProjectId);
                throw new BadRequestException($"Cannot register Time for Project {request.ProjectId}.");
            }
            return project;
        }
        public bool IsTimeRecordInPast(CreateTimeRecordCommand request)
        {
            DateTime currentDateTime = DateTime.Now;

            bool isInPast = request.StartTime.Date <= currentDateTime.Date;

            if (!isInPast)
            {
                _logger.LogWarning("RequestId: {id} - Bad request: Attempt to register future TimeRecord: (start: {start}, duaration: {duration}) on a Project {projectId}.",
                                    request.RequestId, request.StartTime, request.DurationMinutes, request.ProjectId);
                throw new BadRequestException("Time Registration cannot be in the future.");
            }
            return true;
        }

        public bool IsTimeRecordWithinProjectPeriod(Project project, CreateTimeRecordCommand request)
        {
            if (request.StartTime >= project.StartTime && request.StartTime.AddMinutes(request.DurationMinutes) <= project.Deadline)
            {
                return true;
            }
            else
            {
                _logger.LogWarning("RequestId: {id} - Bad request: ProjectId {projectId} - Time registration is outside the project time period.", request.RequestId, project.Id);
                throw new BadRequestException("Time registration is outside the project time period");
            }
        }

    }
}
