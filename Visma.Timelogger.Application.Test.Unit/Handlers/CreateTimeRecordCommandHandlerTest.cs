using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Visma.Timelogger.Application.Contracts;
using Visma.Timelogger.Application.Exceptions;
using Visma.Timelogger.Application.Features.CreateTimeRecord;
using Visma.Timelogger.Application.RequestModels;
using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Application.Test.Unit.Handlers
{
    [TestFixture]
    public class CreateTimeRecordCommandHandlerTests
    {
        private CreateTimeRecordCommandHandler _SUT;
        private Mock<AbstractValidator<CreateTimeRecordCommand>> _commandValidator;
        private Mock<ILogger<CreateTimeRecordCommandHandler>> _loggerMock;
        private Mock<IApiRequestValidator> _validatorMock;
        private Mock<IProjectRepository> _projectRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IEventBusService> _eventBusServiceMock;
        private Project _existingProject;
        private DateTime _now = DateTime.UtcNow.Date;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<CreateTimeRecordCommandHandler>>();
            _commandValidator = new Mock<AbstractValidator<CreateTimeRecordCommand>>();
            _validatorMock = new Mock<IApiRequestValidator>();
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _eventBusServiceMock = new Mock<IEventBusService>();
            _mapperMock = new Mock<IMapper>();
            _existingProject = new Project()
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Deadline = _now.AddDays(2),
                FreelancerId = Guid.NewGuid(),
                IsActive = true,
                StartTime = _now.AddDays(-2)
            };

            _SUT = new CreateTimeRecordCommandHandler(
                _loggerMock.Object,
                _commandValidator.Object,
                _validatorMock.Object,
                _projectRepositoryMock.Object,
                _eventBusServiceMock.Object,
                _mapperMock.Object
            );
        }

        [Test]
        public async Task GivenValidRequest_ProjectExist_ReturnsTrue()
        {
            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel()
            {
                ProjectId = _existingProject.Id
            };
            CreateTimeRecordCommand request = new CreateTimeRecordCommand(requestModel, _existingProject.FreelancerId);
            _projectRepositoryMock.Setup(repo => repo
                                        .GetActiveByProjectIdForFreelancerAsync(_existingProject.Id, _existingProject.FreelancerId))
                                        .ReturnsAsync(_existingProject);

            var result = await _SUT.ProjectExists(request);
            Assert.IsInstanceOf<Project>(result);
        }

        [Test]
        public void GivenInvalidRequest_ProjectExist_ThrowsException()
        {
            Project? x = null;
            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel()
            {
                ProjectId = _existingProject.Id
            };
            CreateTimeRecordCommand request = new CreateTimeRecordCommand(requestModel, _existingProject.FreelancerId);
            _projectRepositoryMock.Setup(repo => repo
                                        .GetActiveByProjectIdForFreelancerAsync(_existingProject.Id, _existingProject.FreelancerId))
                                        .ReturnsAsync(x);

            var exception = Assert.ThrowsAsync<BadRequestException>(async () => await _SUT.ProjectExists(request));
            Assert.That(exception.Message.Equals($"Cannot register Time for Project {request.ProjectId}."));
        }

        [Test]
        public void GivenValidStartTime_IsTimeRecordWithinProjectPeriodTest_ReturnsTrue()
        {
            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel()
            {
                StartTime = _now
            };
            CreateTimeRecordCommand request = new CreateTimeRecordCommand(requestModel, Guid.Empty);
            var result = _SUT.IsTimeRecordWithinProjectPeriod(_existingProject, request);
            Assert.True(result);
        }

        [Test]
        public void GivenValidStartTimeLeftEdge_IsTimeRecordWithinProjectPeriodTest_ReturnsTrue()
        {
            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel()
            {
                StartTime = _now.AddDays(-2)
            };
            CreateTimeRecordCommand request = new CreateTimeRecordCommand(requestModel, Guid.Empty);
            var result = _SUT.IsTimeRecordWithinProjectPeriod(_existingProject, request);
            Assert.True(result);
        }

        [Test]
        public void GivenValidStartTimeRightEdge_IsTimeRecordWithinProjectPeriod_ReturnsTrue()
        {
            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel()
            {
                StartTime = _now.AddDays(2)
            };
            CreateTimeRecordCommand request = new CreateTimeRecordCommand(requestModel, Guid.Empty);
            var result = _SUT.IsTimeRecordWithinProjectPeriod(_existingProject, request);
            Assert.True(result);
        }

        [Test]
        public void GivenInvalidStartTimeLeftEdge_IsTimeRecordWithinProjectPeriod_ThrowsException()
        {
            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel()
            {
                StartTime = _now.AddDays(-3)
            };
            CreateTimeRecordCommand request = new CreateTimeRecordCommand(requestModel, Guid.Empty);
            var exception = Assert.Throws<BadRequestException>(() => _SUT.IsTimeRecordWithinProjectPeriod(_existingProject, request));
            Assert.That(exception.Message.Equals("Time registration is outside the project time period"));
        }

        [Test]
        public void GivenInvalidStartTimeRightEdge_IsTimeRecordWithinProjectPeriod_ThrowsException()
        {
            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel()
            {
                StartTime = _now.AddDays(3)
            };
            CreateTimeRecordCommand request = new CreateTimeRecordCommand(requestModel, Guid.Empty);
            var exception = Assert.Throws<BadRequestException>(() => _SUT.IsTimeRecordWithinProjectPeriod(_existingProject, request));
            Assert.That(exception.Message.Equals("Time registration is outside the project time period"));
        }

        [Test]
        public void GivenPastTimeRecord_IsTimeRecordInPast_ReturnsTrue()
        {
            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel()
            {
                StartTime = _now.AddDays(-10)
            };
            CreateTimeRecordCommand request = new CreateTimeRecordCommand(requestModel, Guid.Empty);

            var result = _SUT.IsTimeRecordInPast(request);

            Assert.True(result);
        }

        [Test]
        public void GivenPresentTimeRecord_IsTimeRecordInPast_ReturnsTrue()
        {
            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel()
            {
                StartTime = _now
            };
            CreateTimeRecordCommand request = new CreateTimeRecordCommand(requestModel, Guid.Empty);

            var result = _SUT.IsTimeRecordInPast(request);

            Assert.True(result);
        }


        [Test]
        public void GivenFutureTimeRecord_IsTimeRecordInPast_ThrowsException()
        {
            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel()
            {
                StartTime = _now.AddDays(5)
            };
            CreateTimeRecordCommand request = new CreateTimeRecordCommand(requestModel, Guid.Empty);

            var exception = Assert.Throws<BadRequestException>(() => _SUT.IsTimeRecordInPast(request));
            Assert.That(exception.Message.Equals("Time Registration cannot be in the future."));
        }

        [Test]
        public async Task GivenValidRequest_Handle_ReturnsTrue()
        {
            var startTime = _now;
            var userId = _existingProject.FreelancerId;
            var projectId = _existingProject.Id;
            var duration = 45;
            var timeRecordId = Guid.NewGuid();

            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel()
            {
                ProjectId = projectId,
                DurationMinutes = duration,
                StartTime = startTime
            };
            CreateTimeRecordCommand request = new CreateTimeRecordCommand(requestModel, userId);

            TimeRecord entity = new TimeRecord()
            {
                Id = timeRecordId,
                ProjectId = projectId,
                DurationMinutes = duration,
                StartTime = startTime, 
                FreelancerId = userId
            };
                 


            _projectRepositoryMock.Setup(repo => repo
                .GetActiveByProjectIdForFreelancerAsync(request.ProjectId, request.UserId))
                .ReturnsAsync(_existingProject);

            _validatorMock.Setup(val => val
                 .ValidateRequest(request, It.IsAny<AbstractValidator<CreateTimeRecordCommand>>(), request.RequestId))
                 .ReturnsAsync(true);

            _mapperMock.Setup(mapper => mapper
                .Map<TimeRecord>(request))
                .Returns(entity);

            _projectRepositoryMock.Setup(repo => repo
                .AddTimeRecordAsync(It.IsAny<Project>())).Verifiable();

            var result = await _SUT.Handle(request, CancellationToken.None);

            Assert.That(result, Is.EqualTo(timeRecordId));
            _validatorMock.Verify(val => val
                 .ValidateRequest(request, It.IsAny<AbstractValidator<CreateTimeRecordCommand>>(), request.RequestId), Times.Once);
            _projectRepositoryMock
                .Verify(repo => repo.GetActiveByProjectIdForFreelancerAsync(request.ProjectId, request.UserId), Times.Once);
            _mapperMock
                .Verify(mapper => mapper.Map<TimeRecord>(request), Times.Once);
            _projectRepositoryMock
                .Verify(repo => repo.AddTimeRecordAsync(It.IsAny<Project>()), Times.Once);
        }

        [Test]
        public void GivenInvalidRequest_Handle_ThrowsException()
        {
            var startTime = _now;
            var userId = _existingProject.FreelancerId;
            var projectId = _existingProject.Id;
            var duration = 5;

            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel()
            {
                ProjectId = projectId,
                DurationMinutes = duration,
                StartTime = startTime
            };
            CreateTimeRecordCommand request = new CreateTimeRecordCommand(requestModel, userId);

            _validatorMock.Setup(val => val
                 .ValidateRequest(request, It.IsAny<AbstractValidator<CreateTimeRecordCommand>>(), request.RequestId))
                 .ThrowsAsync(new RequestValidationException(new FluentValidation.Results.ValidationResult()));

            _projectRepositoryMock.Setup(repo => repo
                .GetActiveByProjectIdForFreelancerAsync(request.ProjectId, request.UserId))
                .ReturnsAsync(_existingProject);

            _mapperMock.Setup(mapper => mapper
                .Map<TimeRecord>(request))
                .Returns(It.IsAny<TimeRecord>());

            _projectRepositoryMock.Setup(repo => repo
                .AddTimeRecordAsync(It.IsAny<Project>())).Verifiable();

            Assert.ThrowsAsync<RequestValidationException>(async () => await _SUT.Handle(request, CancellationToken.None));
            _validatorMock.Verify(val => val
                .ValidateRequest(request, It.IsAny<AbstractValidator<CreateTimeRecordCommand>>(), request.RequestId), Times.Once);
            _projectRepositoryMock
                .Verify(repo => repo.GetActiveByProjectIdForFreelancerAsync(request.ProjectId, request.UserId), Times.Never);
            _mapperMock
                .Verify(mapper => mapper.Map<TimeRecord>(request), Times.Never);
            _projectRepositoryMock
                .Verify(repo => repo.AddTimeRecordAsync(It.IsAny<Project>()), Times.Never);

        }


        [Test]
        public void GivenInvalidProjectData_Handle_ThrowsException()
        {
            var startTime = _now;
            var userId = _existingProject.FreelancerId;
            var projectId = _existingProject.Id;
            var duration = 5;

            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel()
            {
                ProjectId = projectId,
                DurationMinutes = duration,
                StartTime = startTime
            };
            CreateTimeRecordCommand request = new CreateTimeRecordCommand(requestModel, userId);

            _validatorMock.Setup(val => val
                 .ValidateRequest(request, It.IsAny<AbstractValidator<CreateTimeRecordCommand>>(), request.RequestId))
                 .ReturnsAsync(true);

            _projectRepositoryMock.Setup(repo => repo
                .GetActiveByProjectIdForFreelancerAsync(request.ProjectId, request.UserId))
                .ReturnsAsync((Project)null);

            _mapperMock.Setup(mapper => mapper
                .Map<TimeRecord>(request))
                .Returns(It.IsAny<TimeRecord>());

            _projectRepositoryMock.Setup(repo => repo
                .AddTimeRecordAsync(It.IsAny<Project>())).Verifiable();

            Assert.ThrowsAsync<BadRequestException>(async () => await _SUT.Handle(request, CancellationToken.None));
            _validatorMock.Verify(val => val
                .ValidateRequest(request, It.IsAny<AbstractValidator<CreateTimeRecordCommand>>(), request.RequestId), Times.Once);
            _projectRepositoryMock
                .Verify(repo => repo.GetActiveByProjectIdForFreelancerAsync(request.ProjectId, request.UserId), Times.Once);
            _mapperMock
                .Verify(mapper => mapper.Map<TimeRecord>(request), Times.Never);
            _projectRepositoryMock
                .Verify(repo => repo.AddTimeRecordAsync(It.IsAny<Project>()), Times.Never);
        }

        [Test]
        public void GivenStartTimeOutsideProjectPeriod_Handle_ThrowsException()
        {
            var startTime = _now.AddDays(-9);
            var userId = _existingProject.FreelancerId;
            var projectId = _existingProject.Id;
            var duration = 5;

            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel()
            {
                ProjectId = projectId,
                DurationMinutes = duration,
                StartTime = startTime
            };
            CreateTimeRecordCommand request = new CreateTimeRecordCommand(requestModel, userId);

            _validatorMock.Setup(val => val
                 .ValidateRequest(request, It.IsAny<AbstractValidator<CreateTimeRecordCommand>>(), request.RequestId))
                 .ReturnsAsync(true);

            _projectRepositoryMock.Setup(repo => repo
                .GetActiveByProjectIdForFreelancerAsync(request.ProjectId, request.UserId))
                .ReturnsAsync(_existingProject);

            _mapperMock.Setup(mapper => mapper
                .Map<TimeRecord>(request))
                .Returns(It.IsAny<TimeRecord>());

            _projectRepositoryMock.Setup(repo => repo
                .AddTimeRecordAsync(It.IsAny<Project>())).Verifiable();

            var exception = Assert.ThrowsAsync<BadRequestException>(async () => await _SUT.Handle(request, CancellationToken.None));
            Assert.That(exception.Message.Equals("Time registration is outside the project time period"));
            _validatorMock.Verify(val => val
                .ValidateRequest(request, It.IsAny<AbstractValidator<CreateTimeRecordCommand>>(), request.RequestId), Times.Once);
            _projectRepositoryMock
                .Verify(repo => repo.GetActiveByProjectIdForFreelancerAsync(request.ProjectId, request.UserId), Times.Once);
            _mapperMock
                .Verify(mapper => mapper.Map<TimeRecord>(request), Times.Never);
            _projectRepositoryMock
                .Verify(repo => repo.AddTimeRecordAsync(It.IsAny<Project>()), Times.Never);
        }

        [Test]
        public void GivenStartTimeInFuture_Handle_ThrowsException()
        {
            var startTime = _now.AddDays(9);
            var userId = _existingProject.FreelancerId;
            var projectId = _existingProject.Id;
            var duration = 5;

            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel()
            {
                ProjectId = projectId,
                DurationMinutes = duration,
                StartTime = startTime
            };
            CreateTimeRecordCommand request = new CreateTimeRecordCommand(requestModel, userId);

            _validatorMock.Setup(val => val
                 .ValidateRequest(request, It.IsAny<AbstractValidator<CreateTimeRecordCommand>>(), request.RequestId))
                 .ReturnsAsync(true);

            _projectRepositoryMock.Setup(repo => repo
                .GetActiveByProjectIdForFreelancerAsync(request.ProjectId, request.UserId))
                .ReturnsAsync(_existingProject);

            _mapperMock.Setup(mapper => mapper
                .Map<TimeRecord>(request))
                .Returns(It.IsAny<TimeRecord>());

            _projectRepositoryMock.Setup(repo => repo
                .AddTimeRecordAsync(It.IsAny<Project>())).Verifiable();

            var exception = Assert.ThrowsAsync<BadRequestException>(async () => await _SUT.Handle(request, CancellationToken.None));
            Assert.That(exception.Message.Equals("Time Registration cannot be in the future."));
            _validatorMock.Verify(val => val
                .ValidateRequest(request, It.IsAny<AbstractValidator<CreateTimeRecordCommand>>(), request.RequestId), Times.Once);
            _projectRepositoryMock
                .Verify(repo => repo.GetActiveByProjectIdForFreelancerAsync(request.ProjectId, request.UserId), Times.Once);
            _mapperMock
                .Verify(mapper => mapper.Map<TimeRecord>(request), Times.Never);
            _projectRepositoryMock
                .Verify(repo => repo.AddTimeRecordAsync(It.IsAny<Project>()), Times.Never);
        }
    }
}