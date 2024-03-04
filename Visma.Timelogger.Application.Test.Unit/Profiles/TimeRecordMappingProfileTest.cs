using AutoMapper;
using Visma.Timelogger.Application.Features.CreateTimeRecord;
using Visma.Timelogger.Application.Profiles;
using Visma.Timelogger.Application.RequestModels;
using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Application.Test.Unit.Profiles
{
    public class TimeRecordMappingProfileTest
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;

        public TimeRecordMappingProfileTest()
        {
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TimeRecordMappingProfile>();
            });
            _mapper = _mapperConfiguration.CreateMapper();
        }

        [Test]
        public void ConfigurationTest()
        {
            _mapperConfiguration.AssertConfigurationIsValid();
            Assert.Pass();
        }

        [Test]
        public void GivenValidCreateTimeRecordCommand_WhenMappingToTimeRecord_TimeRecordCreated()
        {
            var startTime = DateTime.UtcNow;
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var duration = 9;

            CreateTimeRecordRequestModel requestModel = new CreateTimeRecordRequestModel()
            {
                ProjectId = projectId,
                DurationMinutes = duration,
                StartTime = startTime,
            };

            CreateTimeRecordCommand source = new CreateTimeRecordCommand(requestModel, userId);

            var destination = _mapper.Map<TimeRecord>(source);

            Assert.IsNotNull(destination);
            Assert.IsInstanceOf<TimeRecord>(destination);
            Assert.That(destination.Id != Guid.Empty);
            Assert.That(destination.FreelancerId, Is.EqualTo(userId));
            Assert.That(destination.ProjectId, Is.EqualTo(projectId));
            Assert.That(destination.StartTime, Is.EqualTo(startTime));
            Assert.That(destination.DurationMinutes, Is.EqualTo(duration));
        }
    }
}
