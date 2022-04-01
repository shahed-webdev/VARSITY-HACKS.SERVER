using AutoMapper;
using Itenso.TimePeriod;
using VARSITY_HACKS.DATA;
using VARSITY_HACKS.Repository;
using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.BusinessLogic;

public class EventCore : Core, IEventCore
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
                return Task.FromResult(
                    new ResponseModel<List<UserCalendarViewModel>>(false, $" {model.EventName} already Exist"));

            return Task.FromResult(_db.UserEvent.Add(registrationId, model));
        }
        catch (Exception e)
        {
            return Task.FromResult(new ResponseModel<List<UserCalendarViewModel>>(false,
                $"{e.Message}. {e.InnerException?.Message ?? ""}"));
        }
    }

    public Task<ResponseModel<bool>> IsEventConflictingAsync(string userName, UserEventAddModel model)
    {
        try
        {
            var registrationId = _db.Registration.RegistrationIdByUserName(userName);
            var data = _db.UserEvent.CalendarList(registrationId);

            var timePeriods = new TimePeriodCollection();

            foreach (var calendarViewModel in data)
            {
                timePeriods.Add(new TimeRange(calendarViewModel.StartDateTime, calendarViewModel.EndDateTime));
            }


            // --- intersection by period ---
            var isConflict = false;
            foreach (DateTime date in EachDate(model.StartDate, model.EndDate))
            {
                if (model.Days.Any(d => d == date.DayOfWeek))
                {
                    var start = date.Add(TimeSpan.Parse(model.StartTime));
                    var end = start.AddMinutes(model.DurationMinute);
                    var intersectionPeriod = new TimeRange(start, end);
                    var periodIntersections = timePeriods.IntersectionPeriods(intersectionPeriod);
                    isConflict = periodIntersections.Any();
                    if (isConflict) break;
                }
            }
            return Task.FromResult(new ResponseModel<bool>(true, "", isConflict));
        }
        catch (Exception e)
        {
            return Task.FromResult(new ResponseModel<bool>(false, $"{e.Message}. {e.InnerException?.Message ?? ""}"));
        }
    }


    public Task<ResponseModel<List<UserEventViewModel>>> GetEventsAsync(string userName, EventType type)
    {
        try
        {
            var registrationId = _db.Registration.RegistrationIdByUserName(userName);
            var data = _db.UserEvent.List(registrationId, type);
            return Task.FromResult(new ResponseModel<List<UserEventViewModel>>(true, "Success", data));
        }
        catch (Exception e)
        {
            return Task.FromResult(new ResponseModel<List<UserEventViewModel>>(false,
                $"{e.Message}. {e.InnerException?.Message ?? ""}"));
        }
    }

    public Task<ResponseModel<EventTypeWiseEventViewModel>> GetTypeWiseEventsAsync(string userName)
    {
        try
        {
            var registrationId = _db.Registration.RegistrationIdByUserName(userName);
            var data = new EventTypeWiseEventViewModel
            {
                School = _db.UserEvent.List(registrationId, EventType.School),
                Personal = _db.UserEvent.List(registrationId, EventType.Personal),
                Work = _db.UserEvent.List(registrationId, EventType.Work)
            };

            return Task.FromResult(new ResponseModel<EventTypeWiseEventViewModel>(true, "Success", data));
        }
        catch (Exception e)
        {
            return Task.FromResult(new ResponseModel<EventTypeWiseEventViewModel>(false,
                $"{e.Message}. {e.InnerException?.Message ?? ""}"));
        }
    }

    public Task<ResponseModel<List<UserCalendarViewModel>>> GetCalendarEventsAsync(string userName)
    {
        try
        {
            var registrationId = _db.Registration.RegistrationIdByUserName(userName);
            var data = _db.UserEvent.CalendarList(registrationId);
            return Task.FromResult(new ResponseModel<List<UserCalendarViewModel>>(true, "Success", data));
        }
        catch (Exception e)
        {
            return Task.FromResult(new ResponseModel<List<UserCalendarViewModel>>(false,
                $"{e.Message}. {e.InnerException?.Message ?? ""}"));
        }
    }

    private IEnumerable<DateTime> EachDate(DateTime from, DateTime to)
    {
        for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
            yield return day;
    }
}