using AutoMapper;
using AutoMapper.QueryableExtensions;
using VARSITY_HACKS.DATA;
using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.Repository;

public class UserEventRepository : Repository, IUserEventRepository
{
    public UserEventRepository(ApplicationDbContext db, IMapper mapper) : base(db, mapper)
    {
    }

    public ResponseModel<List<UserCalendarViewModel>> Add(int registrationId, UserEventAddModel model)
    {
        var userEvent = _mapper.Map<UserEvent>(model);
        userEvent.RegistrationId = registrationId;
        Db.UserEvents.Add(userEvent);
        Db.SaveChanges();

        var userCalendarEvents = new List<UserCalendarEvent>();
        foreach (DateTime date in EachDate(userEvent.StartDate, userEvent.EndDate))
        {
            if (userEvent.Days.Any(d => d.Day == date.DayOfWeek))
            {
                var userCalendarEvent = _mapper.Map<UserCalendarEvent>(userEvent);
                userCalendarEvent.EventDate = date;

                userCalendarEvents.Add(userCalendarEvent);
            }
        }

        Db.UserCalendarEvents.AddRange(userCalendarEvents);
        Db.SaveChanges();

        var returnModel = Db.UserCalendarEvents.Where(r => r.UserEventId == userEvent.UserEventId)
            .ProjectTo<UserCalendarViewModel>(_mapper.ConfigurationProvider).ToList();

        return new ResponseModel<List<UserCalendarViewModel>>(true, $"{model.EventName} Added Successfully",
            returnModel);
    }

    public ResponseModel<UserEventViewModel> Get(int id)
    {
        var userEven = Db.UserEvents.Where(r => r.UserEventId == id)
            .ProjectTo<UserEventViewModel>(_mapper.ConfigurationProvider)
            .FirstOrDefault();
        return new ResponseModel<UserEventViewModel>(true, $"{userEven!.EventName} Get Successfully", userEven);
    }

    public bool IsExistName(int registrationId, string name)
    {
        return Db.UserEvents.Any(r => r.RegistrationId == registrationId && r.EventName == name);
    }

    public bool IsExistName(int registrationId, string name, int updateId)
    {
        return Db.UserEvents.Any(r =>
            r.RegistrationId == registrationId && r.EventName == name && r.UserEventId != updateId);
    }

    public bool IsNull(int id)
    {
        return !Db.UserEvents.Any(r => r.UserEventId == id);
    }

    public List<UserEventViewModel> List(int registrationId, EventType type)
    {
        return Db.UserEvents.Where(m => m.RegistrationId == registrationId && m.EventType == type)
            .ProjectTo<UserEventViewModel>(_mapper.ConfigurationProvider)
            .OrderBy(a => a.EventName)
            .ToList();
    }

    public List<UserCalendarViewModel> CalendarList(int registrationId)
    {
        return Db.UserCalendarEvents.Where(m => m.RegistrationId == registrationId)
            .ProjectTo<UserCalendarViewModel>(_mapper.ConfigurationProvider)
            .OrderBy(a => a.EventName)
            .ToList();
    }

    private IEnumerable<DateTime> EachDate(DateTime from, DateTime to)
    {
        for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
            yield return day;
    }
}