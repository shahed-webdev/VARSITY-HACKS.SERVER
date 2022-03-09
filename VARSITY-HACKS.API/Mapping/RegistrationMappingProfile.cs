using AutoMapper;
using VARSITY_HACKS.DATA;
using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.API;

public class RegistrationMappingProfile: Profile
{
    public RegistrationMappingProfile()
    {
        CreateMap<Registration, RegistrationEditModel>()
            .ForMember(d => d.Personality, opt => opt.MapFrom(c => c.Personality.ToString()))
            .ForMember(d => d.PersonalityDescription, opt => opt.MapFrom(c => c.Personality.GetDescription()))
            .ForMember(d => d.Mode, opt => opt.MapFrom(c => c.Mode.ToString()))
            .ReverseMap();
        CreateMap<RegistrationCreateModel, Registration>()
            .ForMember(d=> d.Email, opt=> opt.MapFrom(c=> c.UserName))
            .ForMember(d=> d.Personality, opt=> opt.MapFrom(c=> PersonalityType.EarlyBird));
    }
}