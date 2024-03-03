using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Persistence.Test.Integration
{
    public static class TestData
    {
        public static readonly DateTime Now = DateTime.Parse("03/03/2024 12:00:00");
        public static readonly Guid ActiveProjectId = Guid.NewGuid();
        public static readonly Guid InactiveProjectId = Guid.NewGuid();
        public static readonly Guid CustomerId = Guid.NewGuid();
        public static readonly Guid FreelancerId = Guid.NewGuid();

        public static Project ActiveProject()
        {
            return new Project()
            {
                CustomerId = CustomerId,
                Deadline = Now.AddDays(2),
                FreelancerId = FreelancerId,
                Id = ActiveProjectId,
                IsActive = true,
                StartTime = Now.AddDays(-2)
            };
        }
        public static Project InactiveProject()
        {
            return new Project()
            {
                CustomerId = CustomerId,
                Deadline = Now.AddDays(2),
                FreelancerId = FreelancerId,
                Id = InactiveProjectId,
                IsActive = false,
                StartTime = Now.AddDays(-2)
            };
        }
    }
}
