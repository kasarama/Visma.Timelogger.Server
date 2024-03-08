using AutoMapper;
using Newtonsoft.Json;
using System.Text.Json;
using Visma.Timelogger.Application.Events;
using Visma.Timelogger.Application.VieModels;
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
        }
        private string MapData(Event source)
        {
            return JsonConvert.SerializeObject(source);
        }
    }
}
