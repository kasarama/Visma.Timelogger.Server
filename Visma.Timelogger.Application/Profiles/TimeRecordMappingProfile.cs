using AutoMapper;
using Visma.Timelogger.Application.Features.CreateTimeRecord;
using Visma.Timelogger.Application.ViewModels;
using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Application.Profiles
{
    public class TimeRecordMappingProfile : Profile
    {
        public TimeRecordMappingProfile()
        {
            CreateMap<CreateTimeRecordCommand, TimeRecord>()
                .ForMember(e => e.Id, opt => opt.MapFrom(c => Guid.NewGuid()))
                .ForMember(e => e.FreelancerId, opt => opt.MapFrom(c => c.UserId))
                .ForMember(e => e.ProjectId, opt => opt.MapFrom(c => c.ProjectId))
                .ForMember(e => e.Project, opt => opt.Ignore())
                .ForMember(e => e.StartTime, opt => opt.MapFrom(c => c.StartTime.Date))
                .ForMember(e => e.DurationMinutes, opt => opt.MapFrom(c => c.DurationMinutes))
                ;

            CreateMap<TimeRecord, TimeRecordViewModel>()
                .ForMember(v => v.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(v => v.StartTime, opt => opt.MapFrom(c => c.StartTime))
                .ForMember(v => v.DurationMinutes, opt => opt.MapFrom(c => c.DurationMinutes))
                ;

        }
    }
}
