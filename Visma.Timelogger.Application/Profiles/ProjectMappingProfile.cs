using AutoMapper;
using Visma.Timelogger.Application.VieModels;
using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Application.Profiles
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Project, ProjectOverviewViewModel>()
               .ForMember(p => p.Id, opt => opt.MapFrom(c => c.Id))
               .ForMember(e => e.FreelancerId, opt => opt.MapFrom(c => c.FreelancerId))
               .ForMember(e => e.CustomerId, opt => opt.MapFrom(c => c.CustomerId))
               .ForMember(e => e.StartTime, opt => opt.MapFrom(c => c.StartTime))
               .ForMember(e => e.Deadline, opt => opt.MapFrom(c => c.Deadline))
               .ForMember(e => e.IsActive, opt => opt.MapFrom(c => c.IsActive))
               .ForMember(e => e.Name, opt => opt.MapFrom(c => System.Web.HttpUtility.HtmlEncode(c.Name)))
               .ForMember(e => e.TimeRegistrations, opt => opt.MapFrom(c => c.TimeRecords))
               ;
        }
    }
}
