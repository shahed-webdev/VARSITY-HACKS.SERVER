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

            var userEventId = _db.UserEvent.AddEventWithCalenderEvents(registrationId, model);

            if (userEventId == 0)
                return Task.FromResult(
                    new ResponseModel<List<UserCalendarViewModel>>(false, $" {model.EventName} Not Added"));

            if (model.EventType == EventType.School)
            {
                var personality = _db.Registration.GetPersonalityType(userName);

                ISuggestedStudy study = new SuggestedStudy(model.StartDate, model.EndDate, model.Days);
                
                study.AddSuggestedStudy(registrationId, userEventId, personality, model.Difficulty, _db);

            }

            var data = _db.UserEvent.GetCalenderEventsById(registrationId, userEventId);

            return Task.FromResult(new ResponseModel<List<UserCalendarViewModel>>(true, $"{model.EventName} Successfully Added", data));


        }
        catch (Exception e)
        {
            return Task.FromResult(new ResponseModel<List<UserCalendarViewModel>>(false,
                $"{e.Message}. {e.InnerException?.Message ?? ""}"));
        }
    }

    public Task<ResponseModel<List<UserCalendarViewModel>>> EditAsync(string userName, UserEventEditModel model)
    {
        try
        {
            var registrationId = _db.Registration.RegistrationIdByUserName(userName);

            if (string.IsNullOrEmpty(model.EventName))
                return Task.FromResult(new ResponseModel<List<UserCalendarViewModel>>(false, "Invalid Data"));


            if (_db.UserEvent.IsExistName(registrationId, model.EventName, model.UserEventId))
                return Task.FromResult(
                    new ResponseModel<List<UserCalendarViewModel>>(false, $" {model.EventName} already Exist"));

            var isEventFound = _db.UserEvent.IsNull(model.UserEventId);
            if (isEventFound)
                return Task.FromResult(
                    new ResponseModel<List<UserCalendarViewModel>>(false, $" {model.EventName} Not Found"));

            //Deleting the old event
            _db.UserEvent.DeleteAllCalendarEvents(registrationId,model.UserEventId);

            //Editing the event
            _db.UserEvent.EditEventWithCalenderEvents(registrationId, model);


            if (model.EventType == EventType.School)
            {
                var personality = _db.Registration.GetPersonalityType(userName);

                ISuggestedStudy study = new SuggestedStudy(model.StartDate, model.EndDate, model.Days);

                study.AddSuggestedStudy(registrationId, model.UserEventId, personality, model.Difficulty, _db);


            }

            var data = _db.UserEvent.GetCalenderEventsById(registrationId, model.UserEventId);

            return Task.FromResult(new ResponseModel<List<UserCalendarViewModel>>(true, $"{model.EventName} successfully updated"  , data));


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
            var data = _db.UserEvent.CalendarList(registrationId, model.StartDate.AddDays(-1), model.EndDate.AddDays(1));

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

    public Task<ResponseModel<bool>> IsEventEditConflictingAsync(string userName, UserEventEditModel model)
    {
        try
        {
            var registrationId = _db.Registration.RegistrationIdByUserName(userName);
            var data = _db.UserEvent.CalendarList(registrationId,model.UserEventId, model.StartDate.AddDays(-1), model.EndDate.AddDays(1));

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

    public Task<ResponseModel> DeleteAsync(string userName, int userEventId)
    {
        try
        {
            var registrationId = _db.Registration.RegistrationIdByUserName(userName);

            return Task.FromResult(_db.UserEvent.Delete(registrationId, userEventId));
        }
        catch (Exception e)
        {
            return Task.FromResult(new ResponseModel(false,
                $"{e.Message}. {e.InnerException?.Message ?? ""}"));
        }
    }

    public Task<ResponseModel> DeleteCalendarEventAsync(string userName, int calendarEventId)
    {
        try
        {
            var registrationId = _db.Registration.RegistrationIdByUserName(userName);

            return Task.FromResult(_db.UserEvent.DeleteCalendarEvent(registrationId, calendarEventId));
        }
        catch (Exception e)
        {
            return Task.FromResult(new ResponseModel(false,
                $"{e.Message}. {e.InnerException?.Message ?? ""}"));
        }
    }

    public Task<ResponseModel<UserCalendarViewModel>> EditCalendarEventAsync(string userName, UserCalendarEventEditModel model)
    {
        try
        {
            var registrationId = _db.Registration.RegistrationIdByUserName(userName);

            return Task.FromResult(_db.UserEvent.EditCalendarEvent(registrationId, model));
        }
        catch (Exception e)
        {
            return Task.FromResult(new ResponseModel<UserCalendarViewModel>(false,
                $"{e.Message}. {e.InnerException?.Message ?? ""}"));
        }
    }

    private IEnumerable<DateTime> EachDate(DateTime from, DateTime to)
    {
        for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
            yield return day;
    }
}