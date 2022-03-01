using AutoMapper;
using VARSITY_HACKS.DATA;
using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.API;

public class EventMappingProfile: Profile
{
    public EventMappingProfile()
    {
        CreateMap<UserEventAddModel, UserEvent>()
            .ForMember(d => d.StartTime, opt => opt.MapFrom(c => TimeSpan.Parse(c.StartTime)))
            .ForMember(d => d.Days, opt => opt.MapFrom(c => c.Days.Select(d => new UserEventDay
            {
                Day = d,
            })));

        CreateMap<UserEvent, UserEventViewModel>()
            .ForMember(d => d.EventType, opt => opt.MapFrom(c => c.EventType.ToString()))
            .ForMember(d => d.Priority, opt => opt.MapFrom(c => c.Priority.ToString()))
            .ForMember(d => d.Difficulty, opt => opt.MapFrom(c => c.Difficulty.ToString()))
            .ReverseMap();
        CreateMap<UserCalendarEvent, UserCalendarViewModel>()
            .ForMember(d => d.EventName, opt => opt.MapFrom(c => c.UserEvent.EventName))
            .ForMember(d => d.BackgroundColor, opt => opt.MapFrom(c => c.UserEvent.BackgroundColor))
            .ForMember(d => d.EventType, opt => opt.MapFrom(c => c.UserEvent.EventType.ToString()))
            .ForMember(d => d.Priority, opt => opt.MapFrom(c => c.Priority.ToString()))
            .ForMember(d => d.Difficulty, opt => opt.MapFrom(c => c.Difficulty.ToString()))

            .ReverseMap();


        CreateMap<UserEvent, UserCalendarEvent>().ReverseMap();


    }
}