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
        public async Task GivenInValidUserId_GetByProjectIdForFreelancerAsync_ReturnsNull()
        {
            var result = await _SUT.GetByIdForFreelancerAsync(TestData.InactiveProjectId, Guid.NewGuid());
            Assert.That(result == null);
        }
    }
}
