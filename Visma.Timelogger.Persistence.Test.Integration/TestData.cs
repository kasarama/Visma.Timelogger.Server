using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Persistence.Test.Integration
{
    public static class TestData
    {
        public static readonly DateTime Now = DateTime.Parse("03/03/2024 12:00:00").Date;
        public static readonly Guid ActiveProjectId = Guid.Parse("8a62e07b-1375-4887-8a6b-ac1e9d53f836");
        public static readonly Guid InactiveProjectId = Guid.Parse("2BE62E76-3F5E-4F57-91A9-C371FB93A664");
        public static readonly Guid CustomerId = Guid.Parse("950C4D6C-2553-43D2-A197-FCEFC68ADB93");
        public static readonly Guid FreelancerId = Guid.Parse("486E3E8F-0DC3-4E00-8711-BE3A6CB1399E");

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
