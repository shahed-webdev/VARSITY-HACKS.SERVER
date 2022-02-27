using AutoMapper;
using AutoMapper.QueryableExtensions;
using VARSITY_HACKS.DATA;
using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.Repository;

public class RegistrationRepository:Repository, IRegistrationRepository
{
    public RegistrationRepository(ApplicationDbContext db, IMapper mapper) : base(db, mapper)
    {
    }

    public int RegistrationIdByUserName(string userName)
    {
        return Db.Registrations.FirstOrDefault(r => r.UserName == userName)?.RegistrationId ?? 0;
    }
    
    public ResponseModel Edit(string userName, RegistrationEditModel model)
    {
        var registration = Db.Registrations.FirstOrDefault(r => r.UserName == userName);
        if (registration == null) return new ResponseModel(false, "data Not Found");

        registration.Name = model.Name;
        registration.City = model.City;
        registration.State = model.State;
        registration.SocialMediaLink = model.SocialMediaLink;
        registration.UniversityName = model.UniversityName;
        registration.Subject = model.Subject;
        registration.Personality = model.Personality;
        registration.Email = model.Email;
        registration.Image = model.Image;
        Db.Registrations.Update(registration);
        Db.SaveChanges();
        return new ResponseModel(true, $"{registration.UserName} Updated Successfully");
    }

    public void Create(string name, string userName)
    {
        var registration = new Registration
        {

            UserName = userName,
            Validation = true,
            Personality = PersonalityType.EarlyBird,
            Name = name,
            Email = userName,
        };
        Db.Registrations.Add(registration);
        Db.SaveChanges();
    }

    public ResponseModel<RegistrationEditModel> Get(string userName)
    {
        var registration = Db.Registrations.Where(r => r.UserName == userName)
            .ProjectTo<RegistrationEditModel>(_mapper.ConfigurationProvider)
            .FirstOrDefault();

        return registration == null
            ? new ResponseModel<RegistrationEditModel>(false, "data Not Found")
            : new ResponseModel<RegistrationEditModel>(true, $"{registration!.Name} Get Successfully",
                registration);
    }
}