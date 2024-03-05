using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Api.Test.Integration.Base
{
    public static class TestData
    {
        public static readonly DateTime Now = DateTime.Parse("03/03/2024 12:00:00").Date;
        public static readonly Guid ActiveProjectId = Guid.Parse("73665E84-707C-49A3-B51B-4B15368DE3BF");
        public static readonly Guid InactiveProjectId = Guid.Parse("F93265EB-EB10-41A9-8A4B-3C06B0F97C73");
        public static readonly Guid CustomerId = Guid.Parse("9E2195C5-9360-4B97-88D2-58581C92196D");
        public static readonly Guid FreelancerId = Guid.Parse("486E3E8F-0DC3-4E00-8711-BE3A6CB1399E"); // must be same as of freelancer1 in Auth Middleware
        public static readonly Project ActiveProject = _ActiveProject();
        public static readonly Project InactiveProject = _InactiveProject();

        private static Project _ActiveProject()
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
        private static Project _InactiveProject()
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
