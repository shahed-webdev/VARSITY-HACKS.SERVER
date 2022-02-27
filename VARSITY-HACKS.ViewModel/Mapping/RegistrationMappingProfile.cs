using AutoMapper;
using VARSITY_HACKS.DATA;

namespace VARSITY_HACKS.ViewModel;

public class RegistrationMappingProfile: Profile
{
    public RegistrationMappingProfile()
    {
        CreateMap<Registration, RegistrationEditModel>().ReverseMap();
        CreateMap<RegistrationCreateModel, Registration>()
            .ForMember(d=> d.Email, opt=> opt.MapFrom(c=> c.UserName))
            .ForMember(d=> d.Personality, opt=> opt.MapFrom(c=> PersonalityType.EarlyBird));
    }
}