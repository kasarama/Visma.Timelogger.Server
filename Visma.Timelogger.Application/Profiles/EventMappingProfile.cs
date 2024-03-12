using AutoMapper;
using Newtonsoft.Json;
using Visma.Timelogger.Application.Events;
using Visma.Timelogger.Application.Events.Pub;
using Visma.Timelogger.Application.Events.Sub;
using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Application.Profiles
{
    public class EventMappingProfile : Profile
    {
        public EventMappingProfile()
        {
            CreateMap<TimeRecord, TimeRecordCreatedEvent>()
               .ForMember(p => p.EventId, opt => opt.MapFrom(c => Guid.NewGuid()))
               .ForMember(p => p.AggregateId, opt => opt.MapFrom(c => c.Id))
               .ForMember(e => e.FreelancerId, opt => opt.MapFrom(c => c.FreelancerId))
               .ForMember(e => e.StartTime, opt => opt.MapFrom(c => c.StartTime))
               .ForMember(e => e.ProjectId, opt => opt.MapFrom(c => c.ProjectId))
               .ForMember(e => e.DurationMinutes, opt => opt.MapFrom(c => c.DurationMinutes))
               ;

            CreateMap<TimeRecordCreatedEvent, DomainEvent>()
                .ForMember(dst => dst.EventName, opt => opt.MapFrom(src => src.GetType().Name))
                .ForMember(dst => dst.IsPublished, opt => opt.MapFrom(_ => false))
                .ForMember(dst => dst.Data, opt => opt.MapFrom(src => MapData(src)))
                ;

            CreateMap<ProjectCreatedEvent, Project>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.AggregateId))
                .ForMember(dst => dst.FreelancerId, opt => opt.MapFrom(src => src.FreelancerId))
                .ForMember(dst => dst.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dst => dst.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dst => dst.Deadline, opt => opt.MapFrom(src => src.Deadline))
                .ForMember(dst => dst.TimeRecords, opt => opt.MapFrom(src => new List<TimeRecord>()))
                .ForMember(dst => dst.IsActive, opt => opt.MapFrom(src => false))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                ;
        }
        private string MapData(Event source)
        {
            return JsonConvert.SerializeObject(source);
        }
    }
}
