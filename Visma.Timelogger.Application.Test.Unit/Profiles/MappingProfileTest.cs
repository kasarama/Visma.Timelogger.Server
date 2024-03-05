using AutoMapper;
using Visma.Timelogger.Application.Features.CreateTimeRecord;
using Visma.Timelogger.Application.Profiles;
using Visma.Timelogger.Application.RequestModels;
using Visma.Timelogger.Application.VieModels;
using Visma.Timelogger.Application.ViewModels;
using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Application.Test.Unit.Profiles
{
    public class MappingProfileTest
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;

        public MappingProfileTest()
        {
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TimeRecordMappingProfile>();
                cfg.AddProfile<ProjectMappingProfile>();
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
            var startTime = DateTime.UtcNow.Date;
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
            Assert.That(destination.Project, Is.Null);
        }

        [Test]
        public void GivenValidTimeRecord_WhenMappingToTimeRecordViewModel_TimeRecordViewModelCreated()
        {
            var Id = Guid.NewGuid();
            var startTime = DateTime.UtcNow.Date;
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var duration = 9;

            TimeRecord source = new TimeRecord()
            {
                ProjectId = projectId,
                DurationMinutes = duration,
                StartTime = startTime,
                FreelancerId = userId,
                Id = Id
            };

            var destination = _mapper.Map<TimeRecordViewModel>(source);

            Assert.IsNotNull(destination);
            Assert.IsInstanceOf<TimeRecordViewModel>(destination);
            Assert.That(destination.Id, Is.EqualTo(Id));
            Assert.That(destination.StartTime, Is.EqualTo(startTime));
            Assert.That(destination.DurationMinutes, Is.EqualTo(duration));
        }

        [Test]
        public void GivenValidProject_WhenMappingToProjectOverviewViewModel_ProjectOverviewViewModelCreated()
        {
            Project source = new Project()
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Deadline = DateTime.UtcNow.Date.AddDays(30),
                FreelancerId= Guid.NewGuid(),
                IsActive = true,
                Name = "<script>function myFunction(){alert('Hello! I am an alert box!');}</script>",
                StartTime = DateTime.UtcNow.Date.AddDays(-3)                
            };

            List<TimeRecord> records = new List<TimeRecord>();
            for (int i = 1; i < 4; i++)
            {
                TimeRecord record = new TimeRecord()
                {
                    ProjectId = source.Id,
                    DurationMinutes = 30 * i,
                    StartTime = DateTime.UtcNow.Date.AddDays(-i),
                    FreelancerId = source.FreelancerId,
                    Id = Guid.NewGuid()
                };
                records.Add(record);
            }

            source.TimeRecords = records;

            var destination = _mapper.Map<ProjectOverviewViewModel>(source);

            Assert.IsNotNull(destination);
            Assert.IsInstanceOf<ProjectOverviewViewModel>(destination);
            Assert.That(destination.Id, Is.EqualTo(source.Id));
            Assert.That(destination.CustomerId, Is.EqualTo(source.CustomerId));
            Assert.That(destination.Deadline, Is.EqualTo(source.Deadline));
            Assert.That(destination.FreelancerId, Is.EqualTo(source.FreelancerId));
            Assert.That(destination.IsActive, Is.EqualTo(source.IsActive));
            Assert.That(destination.Name.Length, Is.GreaterThan(source.Name.Length));
            Assert.That(destination.StartTime, Is.EqualTo(source.StartTime));
            Assert.That(destination.TimeRegistrations.Count, Is.EqualTo(source.TimeRecords.Count));
        }

    }
}
