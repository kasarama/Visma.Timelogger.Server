using Microsoft.Extensions.Logging;
using Moq;
using Visma.Timelogger.Application.Exceptions;
using Visma.Timelogger.Application.Features.CreateTimeRecord;
using Visma.Timelogger.Application.RequestModels;
using Visma.Timelogger.Application.Services;

namespace Visma.Timelogger.Application.Test.Unit.Services
{
    public class RequestValidatorTest
    {
        private RequestValidator _SUT;
        private Mock<ILogger<RequestValidator>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<RequestValidator>>();
            _SUT = new RequestValidator(_loggerMock.Object);
        }

        [Test]
        public async Task GivenValidCreateTimeRecordCommand_WhenValidateRequest_ReturnsTrue()
        {
            var startTime = DateTime.UtcNow;
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
            var startTime = DateTime.UtcNow;
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
            var startTime = DateTime.UtcNow;
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
    }
}
