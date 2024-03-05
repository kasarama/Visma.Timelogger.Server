using Microsoft.EntityFrameworkCore;
using Visma.Timelogger.Domain.Entities;
using Visma.Timelogger.Persistence.Repositories;

namespace Visma.Timelogger.Persistence.Test.Integration
{
    public class ProjectRepositoryTest
    {
        private ProjectRepository _SUT;
        private TestDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ProjectDbContext>()
            .UseInMemoryDatabase(databaseName: "ProjectRepositoryTestDatabase")
            .Options;
            _context = new TestDbContext(options);
            _SUT = new ProjectRepository(_context);
            _context.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public async Task GivenValidData_GetActiveByProjectIdForFreelancerAsync_ReturnsProject()
        {
            var result = await _SUT.GetActiveByProjectIdForFreelancerAsync(TestData.ActiveProjectId, TestData.FreelancerId);
            Assert.That(result != null);
            Assert.That(result.IsActive);
            Assert.That(result.Id.Equals(TestData.ActiveProjectId));
            Assert.That(result.FreelancerId.Equals(TestData.FreelancerId));
        }

        [Test]
        public async Task GivenInactiveProject_GetActiveByProjectIdForFreelancerAsync_ReturnsNull()
        {
            var result = await _SUT.GetActiveByProjectIdForFreelancerAsync(TestData.InactiveProjectId, TestData.FreelancerId);
            Assert.That(result == null);
        }
        [Test]
        public async Task GivenInvalidProjectId_GetActiveByProjectIdForFreelancerAsync_ReturnsNull()
        {
            var result = await _SUT.GetActiveByProjectIdForFreelancerAsync(Guid.NewGuid(), TestData.FreelancerId);
            Assert.That(result == null);
        }

        [Test]
        public async Task GivenInvalidFreelancerId_GetActiveByProjectIdForFreelancerAsync_ReturnsNull()
        {
            var result = await _SUT.GetActiveByProjectIdForFreelancerAsync(TestData.InactiveProjectId, Guid.NewGuid());
            Assert.That(result == null);
        }


        [Test]
        public async Task GivenExistingProjectWithNewTimeRecord_AddTimeRecordAsync_RecordSaved()
        {
            var project = _context.Projects.First();
            TimeRecord tr = new TimeRecord()
            {
                DurationMinutes = 100,
                ProjectId = project.Id,
                FreelancerId = project.FreelancerId,
                StartTime = DateTime.UtcNow.Date,
            };
            project.TimeRecords.Add(tr);

            await _SUT.AddTimeRecordAsync(project);

            var updated = _context.Projects.Where(e => e.Id == project.Id).FirstOrDefault();
            Assert.That(updated.Id.Equals(project.Id));
            Assert.That(updated.TimeRecords.Find(t => t.Id == tr.Id).Id.Equals(tr.Id));
        }

        [Test]
        public async Task GivenValidData_GetByProjectIdForFreelancerAsync_ReturnsProject()
        {
            var result = await _SUT.GetByIdForFreelancerAsync(TestData.InactiveProjectId, TestData.FreelancerId);
            Assert.That(result != null);
            Assert.That(!result.IsActive);
            Assert.That(result.Id.Equals(TestData.InactiveProjectId));
            Assert.That(result.FreelancerId.Equals(TestData.FreelancerId));
        }

        [Test]
        public async Task GivenInValidProjectId_GetByProjectIdForFreelancerAsync_ReturnsNull()
        {
            var result = await _SUT.GetByIdForFreelancerAsync(Guid.NewGuid(), TestData.FreelancerId);
            Assert.That(result == null);
        }

        [Test]
        public async Task GivenInvalidUserId_GetByIdForFreelancerAsync_ReturnsNull()
        {
            var result = await _SUT.GetByIdForFreelancerAsync(TestData.InactiveProjectId, Guid.NewGuid());
            Assert.That(result == null);
        }

        [Test]
        public async Task GivenValidUserId_GetListForFreelancerAsync_ReturnsList()
        {
            List<Project> testProjects = new List<Project>();
            int count = 3;
            Guid freelancerId = Guid.NewGuid();

            for (int j = 0; j < count; j++)
            {
                Project project = new Project()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = Guid.NewGuid(),
                    Deadline = DateTime.UtcNow.Date.AddDays(30),
                    FreelancerId = freelancerId,
                    IsActive = true,
                    Name = "<script>function myFunction(){alert('Hello! I am an alert box!');}</script>",
                    StartTime = DateTime.UtcNow.Date.AddDays(-3)
                };

                List<TimeRecord> records = new List<TimeRecord>();
                for (int i = 1; i <= count ; i++)
                {
                    TimeRecord record = new TimeRecord()
                    {
                        ProjectId = project.Id,
                        DurationMinutes = 30 * i,
                        StartTime = DateTime.UtcNow.Date.AddDays(-i),
                        FreelancerId = freelancerId,
                        Id = Guid.NewGuid()
                    };

                    records.Add(record);
                }

                project.TimeRecords = records;
                testProjects.Add(project);
            }

            _context.Projects.AddRange(testProjects);
            _context.SaveChanges();

            List<Project> result = await _SUT.GetListForFreelancerAsync(freelancerId);

            Assert.That(result.Count, Is.EqualTo(count));
            Assert.That(result.ElementAt(0).TimeRecords.Count, Is.EqualTo(count));
        }

        [Test]
        public async Task GivenValidUserId_GetListForFreelancerAsync_ReturnsOrderedList()
        {
            List<Project> testProjects = new List<Project>();
            int count = 3;
            Guid freelancerId = Guid.NewGuid();

            for (int j = 0; j < count; j++)
            {
                Project project = new Project()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = Guid.NewGuid(),
                    Deadline = DateTime.UtcNow.Date.AddDays(30-j),
                    FreelancerId = freelancerId,
                    IsActive = true,
                    Name = "<script>function myFunction(){alert('Hello! I am an alert box!');}</script>",
                    StartTime = DateTime.UtcNow.Date.AddDays(-3)
                };

                List<TimeRecord> records = new List<TimeRecord>();
                for (int i = 1; i <= count; i++)
                {
                    TimeRecord record = new TimeRecord()
                    {
                        ProjectId = project.Id,
                        DurationMinutes = 30 * i,
                        StartTime = DateTime.UtcNow.Date.AddDays(-i),
                        FreelancerId = freelancerId,
                        Id = Guid.NewGuid()
                    };

                    records.Add(record);
                }

                project.TimeRecords = records;
                testProjects.Add(project);
            }

            _context.Projects.AddRange(testProjects);
            _context.SaveChanges();

            List<Project> result = await _SUT.GetListForFreelancerAsync(freelancerId);

            Assert.That(result.ElementAt(0).Deadline, Is.LessThan(result.ElementAt(1).Deadline));
            Assert.That(result.ElementAt(0).TimeRecords.Count, Is.EqualTo(count));
        }

        [Test]
        public async Task GivenInvalidUserId_GetListForFreelancerAsync_ReturnsEmptyList()
        {
            var result = await _SUT.GetListForFreelancerAsync(Guid.NewGuid());
            Assert.That(result, Is.Empty);
        }
    }

}
