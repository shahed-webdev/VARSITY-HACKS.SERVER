using AutoMapper;
using VARSITY_HACKS.Repository;
using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.BusinessLogic;

public class EventCore :Core, IEventCore
{
    public EventCore(IUnitOfWork db, IMapper mapper) : base(db, mapper)
    {
    }

    public Task<ResponseModel<List<UserCalendarViewModel>>> AddAsync(string userName, UserEventAddModel model)
    {
        try
        {
            var registrationId = _db.Registration.RegistrationIdByUserName(userName);

            if (string.IsNullOrEmpty(model.EventName))
                return Task.FromResult(new ResponseModel<List<UserCalendarViewModel>>(false, "Invalid Data"));



            if (_db.UserEvent.IsExistName(registrationId, model.EventName))
                return Task.FromResult(new ResponseModel<List<UserCalendarViewModel>>(false, $" {model.EventName} already Exist"));

            return Task.FromResult(_db.UserEvent.Add(model));

        }
        catch (Exception e)
        {
            return Task.FromResult(new ResponseModel<List<UserCalendarViewModel>>(false, $"{e.Message}. {e.InnerException?.Message ?? ""}"));
        }
    }

    public Task<ResponseModel<List<UserEventViewModel>>> GetEventsAsync(string userName)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<List<UserCalendarViewModel>>> GetCalendarEventsAsync(string userName)
    {
        throw new NotImplementedException();
    }
}