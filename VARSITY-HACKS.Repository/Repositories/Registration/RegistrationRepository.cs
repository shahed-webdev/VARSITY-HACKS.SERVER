﻿using AutoMapper;
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
    
    public ResponseModel<RegistrationEditModel> Edit(string userName, RegistrationEditModel model)
    {
        var registration = Db.Registrations.FirstOrDefault(r => r.UserName == userName);
        if (registration == null) return new ResponseModel<RegistrationEditModel>(false, "data Not Found");

        registration.Name = model.Name;
        registration.City = model.City;
        registration.State = model.State;
        registration.SocialMediaLink = model.SocialMediaLink;
        registration.UniversityName = model.UniversityName;
        registration.Subject = model.Subject;
        registration.Personality = (PersonalityType)Enum.Parse(typeof(PersonalityType), model.Personality, true); 
        registration.Email = model.Email;
        if(model.Image != null)
            registration.Image = model.Image;
        Db.Registrations.Update(registration);
        Db.SaveChanges();

        var registrationModel = _mapper.Map<RegistrationEditModel>(registration);
        return new ResponseModel<RegistrationEditModel>(true, $"{registration.UserName} Updated Successfully", registrationModel);
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
            : new ResponseModel<RegistrationEditModel>(true, $"{registration!.Name} Get Successfully", registration);
    }

    public ResponseModel<string> GetMode(string userName)
    {
        var registration = Db.Registrations.FirstOrDefault(r => r.UserName == userName);

        return registration == null
            ? new ResponseModel<string>(false, "data Not Found")
            : new ResponseModel<string>(true, $"Get Successfully", registration.Mode.ToString());
    }

    public ResponseModel<string> SetMode(string userName, UserMode mode)
    {
        var registration = Db.Registrations.FirstOrDefault(r => r.UserName == userName);
        if (registration == null) return new ResponseModel<string>(false, "data Not Found");

        registration.Mode = mode;
        
        Db.Registrations.Update(registration);
        Db.SaveChanges();
        return new ResponseModel<string>(true, $"Update Successfully", registration.Mode.ToString());
    }

    public PersonalityType GetPersonalityType(string userName)
    {
        var registration = Db.Registrations.FirstOrDefault(r => r.UserName == userName);
        return registration?.Personality ?? PersonalityType.EarlyBird;
    }
}