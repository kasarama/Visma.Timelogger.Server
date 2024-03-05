using Microsoft.Extensions.Logging;
using Moq;
using Visma.Timelogger.Application.Exceptions;
using Visma.Timelogger.Application.Features.CreateTimeRecord;
using Visma.Timelogger.Application.Features.GetProjectOverview;
using Visma.Timelogger.Application.RequestModels;
using Visma.Timelogger.Application.Services;

namespace Visma.Timelogger.Application.Test.Unit.Services
{
    public class HandlerServiceTest
    {
        private HandlerService _SUT;
        private Mock<ILogger<HandlerService>> _loggerMock;
        private readonly DateTime _now = DateTime.Now.Date;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<HandlerService>>();
            _SUT = new HandlerService(_loggerMock.Object);
        }

        [Test]
        public async Task GivenValidCreateTimeRecordCommand_WhenValidateRequest_ReturnsTrue()
        {
            var startTime = _now;
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var duration = 60 * 24;

            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel()
            {
                ProjectId = projectId,
                DurationMinutes = duration,
                StartTime = startTime,

            };
            CreateTimeRecordCommand request = new CreateTimeRecordCommand(requestModel, userId);
            CreateTimeRecordCommandValidator validator = new CreateTimeRecordCommandValidator();

            var result = await _SUT.ValidateRequest(request, validator, request.RequestId);
            Assert.True(result);
        }

        [Test]
        public void GivenToShortDuration_WhenValidateRequest_ThrowsException()
        {
            var startTime = _now;
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var duration = 15;

            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel()
            {
                ProjectId = projectId,
                DurationMinutes = duration,
                StartTime = startTime,

            };
            CreateTimeRecordCommand request = new CreateTimeRecordCommand(requestModel, userId);
            CreateTimeRecordCommandValidator validator = new CreateTimeRecordCommandValidator();


            var exception = Assert.ThrowsAsync<RequestValidationException>(async () => await _SUT.ValidateRequest(request, validator, request.RequestId));
            Assert.That(exception.ValidationErrors.Contains("Duration must be of at least 30 minutes."));
        }

        [Test]
        public void GivenToLongDuration_WhenValidateRequest_ThrowsException()
        {
            var startTime = _now;
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var duration = 60 * 24 + 15;

            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel()
            {
                ProjectId = projectId,
                DurationMinutes = duration,
                StartTime = startTime,

            };
            CreateTimeRecordCommand request = new CreateTimeRecordCommand(requestModel, userId);
            CreateTimeRecordCommandValidator validator = new CreateTimeRecordCommandValidator();

            var exception = Assert.ThrowsAsync<RequestValidationException>(async () => await _SUT.ValidateRequest(request, validator, request.RequestId));
            Assert.That(exception.ValidationErrors.Contains("Duration must be of max 24 hours."));
        }

        [Test]
        public void GivenEmptyProperties_WhenValidateRequest_ThrowsExceptionWithValidationErrors()
        {
            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel() { };
            CreateTimeRecordCommand request = new CreateTimeRecordCommand(requestModel, Guid.Empty);
            CreateTimeRecordCommandValidator validator = new CreateTimeRecordCommandValidator();

            var exception = Assert.ThrowsAsync<RequestValidationException>(async () => await _SUT.ValidateRequest(request, validator, request.RequestId));
            Assert.That(exception.ValidationErrors.Contains("Freelancer Id is required."));
            Assert.That(exception.ValidationErrors.Contains("Project Id is required."));
            Assert.That(exception.ValidationErrors.Contains("Duration Minutes is required."));
            Assert.That(exception.ValidationErrors.Contains("Start Time is required."));
        }

        [Test]
        public async Task GivenValidGetProjectOverviewQuery_WhenValidateRequest_ReturnsTrue()
        {
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            
            GetProjectOverviewQuery request = new GetProjectOverviewQuery(projectId, userId);
            GetProjectOverviewQueryValidator validator = new GetProjectOverviewQueryValidator();

            var result = await _SUT.ValidateRequest(request, validator, request.RequestId);
            Assert.True(result);
        }

        [Test]
        public void GivenInvalidProjectId_WhenValidateRequest_ThrowsException()
        {
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            GetProjectOverviewQuery request = new GetProjectOverviewQuery(Guid.Empty, userId);
            GetProjectOverviewQueryValidator validator = new GetProjectOverviewQueryValidator();

            var exception = Assert.ThrowsAsync<RequestValidationException>(async () => await _SUT.ValidateRequest(request, validator, request.RequestId));
            Assert.That(exception.ValidationErrors.Contains("Project Id is required.")); ;
        }

        [Test]
        public void GivenInvalidUserId_WhenValidateRequest_ThrowsException()
        {
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            GetProjectOverviewQuery request = new GetProjectOverviewQuery(projectId, Guid.Empty);
            GetProjectOverviewQueryValidator validator = new GetProjectOverviewQueryValidator();

            var exception = Assert.ThrowsAsync<RequestValidationException>(async () => await _SUT.ValidateRequest(request, validator, request.RequestId));
            Assert.That(exception.ValidationErrors.Contains("Freelancer Id is required."));
        }
    }
}
