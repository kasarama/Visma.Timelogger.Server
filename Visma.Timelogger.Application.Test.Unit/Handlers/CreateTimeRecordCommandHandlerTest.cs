using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel.DataAnnotations;
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
        private Mock<IRequestValidator> _validatorMock;
        private Mock<IProjectRepository> _projectRepositoryMock;
        private Mock<IAsyncRepository<TimeRecord>> _timeRecordRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Project _existingProject;
        private DateTime _now = DateTime.Now;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<CreateTimeRecordCommandHandler>>();
            _commandValidator = new Mock<AbstractValidator<CreateTimeRecordCommand>>();
            _validatorMock = new Mock<IRequestValidator>();
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _timeRecordRepositoryMock = new Mock<IAsyncRepository<TimeRecord>>();
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
                _timeRecordRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Test]
        public async Task GivenValidRequest_WhenProjectExist_ReturnsTrue()
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
        public void GivenInvalidRequest_WhenProjectExist_ThrowsException()
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
        public void GivenValidStartTime_WhenIsTimeRecordWithinProjectPeriodTest_ReturnsTrue()
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
        public void GivenValidStartTimeLeftEdge_WhenIsTimeRecordWithinProjectPeriodTest_ReturnsTrue()
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
        public void GivenValidStartTimeRightEdge_WhenIsTimeRecordWithinProjectPeriod_ReturnsTrue()
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
        public void GivenInvalidStartTimeLeftEdge_WhenIsTimeRecordWithinProjectPeriod_ThrowsException()
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
        public void GivenInvalidStartTimeRightEdge_WhenIsTimeRecordWithinProjectPeriod_ThrowsException()
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
        public void GivenPastTimeRecord_WhenIsTimeRecordInPast_ReturnsTrue()
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
        public void GivenPresentTimeRecord_WhenIsTimeRecordInPast_ReturnsTrue()
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
        public void GivenFutureTimeRecord_WhenIsTimeRecordInPast_ThrowsException()
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
        public async Task GivenValidRequest_WhenHandlingCommand_ReturnsTrue()
        {
            var startTime = _now;
            var userId = _existingProject.FreelancerId;
            var projectId = _existingProject.Id;
            var duration = 45;

            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel()
            {
                ProjectId = projectId,
                DurationMinutes = duration,
                StartTime = startTime
            };
            CreateTimeRecordCommand request = new CreateTimeRecordCommand(requestModel, userId);

            //_loggerMock.Object
            //_commandValidator.Object,
            //_validatorMock.Object, *
            //_projectRepositoryMock.Object, *
            //_timeRecordRepositoryMock.Object, *
            //_mapperMock.Object

            _projectRepositoryMock.Setup(repo => repo
                .GetActiveByProjectIdForFreelancerAsync(request.ProjectId, request.FreelancerId))
                .ReturnsAsync(_existingProject);

            _validatorMock.Setup(val => val
                 .ValidateRequest(request, It.IsAny<AbstractValidator<CreateTimeRecordCommand>>(), request.RequestId))
                 .ReturnsAsync(true);


            _mapperMock.Setup(mapper => mapper
                .Map<TimeRecord>(request))
                .Returns(It.IsAny<TimeRecord>());

            _timeRecordRepositoryMock.Setup(repo => repo
                .AddAsync(It.IsAny<TimeRecord>()))
                .ReturnsAsync(It.IsAny<TimeRecord>());

            var result = await _SUT.Handle(request, CancellationToken.None);

            Assert.True(result);
            _validatorMock.Verify(val => val
                 .ValidateRequest(request, It.IsAny<AbstractValidator<CreateTimeRecordCommand>>(), request.RequestId), Times.Once);
            _projectRepositoryMock
                .Verify(repo => repo.GetActiveByProjectIdForFreelancerAsync(request.ProjectId, request.FreelancerId), Times.Once);
            _mapperMock
                .Verify(mapper => mapper.Map<TimeRecord>(request), Times.Once);
            _timeRecordRepositoryMock.Verify(repo => repo
                .AddAsync(It.IsAny<TimeRecord>()), Times.Once);
        }

        [Test]
        public void GivenInvalidDuration_WhenHandlingCommand_ThrowsException()
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
            _projectRepositoryMock.Setup(repo => repo
                .GetActiveByProjectIdForFreelancerAsync(request.ProjectId, request.FreelancerId))
                .ReturnsAsync(_existingProject);

            _validatorMock.Setup(val => val
                .ValidateRequest(request, It.IsAny<AbstractValidator<CreateTimeRecordCommand>>(), request.RequestId))
                .ThrowsAsync(new RequestValidationException(new FluentValidation.Results.ValidationResult()));

            Assert.ThrowsAsync<RequestValidationException>(async () => await _SUT.Handle(request, CancellationToken.None));
        }

        [Test]
        public void GivenInvalidStartTime_WhenHandlingCommand_ThrowsException()
        {
            var startTime = _now.AddDays(5);
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
            _projectRepositoryMock.Setup(repo => repo
                .GetActiveByProjectIdForFreelancerAsync(request.ProjectId, request.FreelancerId))
                .ReturnsAsync(_existingProject);
            _validatorMock.Setup(val => val
                .ValidateRequest(request, It.IsAny<AbstractValidator<CreateTimeRecordCommand>>(), request.RequestId))
                .ReturnsAsync(true);

            var x = Assert.ThrowsAsync<BadRequestException>(async () => await _SUT.Handle(request, CancellationToken.None));
            Assert.Pass();
        }

        [Test]
        public void _GivenInvalidStartTime_WhenHandlingCommand_ThrowsException()
        {
            Project? project = null;
            var startTime = _now.AddDays(5);
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
            _projectRepositoryMock.Setup(repo => repo
                .GetActiveByProjectIdForFreelancerAsync(request.ProjectId, request.FreelancerId))
                .ReturnsAsync(_existingProject);

            Assert.ThrowsAsync<BadRequestException>(async () => await _SUT.Handle(request, CancellationToken.None));
        }
    }
}