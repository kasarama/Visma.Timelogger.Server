using Visma.Timelogger.Application.ViewModels;

namespace Visma.Timelogger.Application.VieModels
{
    public class ProjectOverviewViewModel
    {
        public Guid Id { get; set; }
        public Guid FreelancerId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime Deadline { get; set; }
        public List<TimeRecordViewModel> TimeRegistrations { get; set; } = new List<TimeRecordViewModel>();
        public bool IsActive { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
