using VARSITY_HACKS.DATA;
using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.Repository;

public interface IUserEventRepository
{
    ResponseModel<List<UserCalendarViewModel>> Add(int registrationId, UserEventAddModel model);
    int AddEventWithCalenderEvents(int registrationId, UserEventAddModel model);
    void AddSuggestedEvents(IEnumerable<UserSuggestedEventAddModel> model);
    List<UserCalendarViewModel> GetCalenderEventsById(int registrationId, int userEventId);
    ResponseModel<UserEventViewModel> Get(int id);
    ResponseModel Delete(int registrationId, int userEventId);
    bool IsExistName(int registrationId, string name);
    bool IsExistName(int registrationId, string name, int updateId);
    bool IsNull(int id);
    List<UserEventViewModel> List(int registrationId, EventType type);
    List<UserCalendarViewModel> CalendarList(int registrationId);
    List<UserCalendarViewModel> CalendarList(int registrationId, DateTime fromDate, DateTime toDate);
    ResponseModel DeleteCalendarEvent(int registrationId, int calendarEventId);
    ResponseModel<UserCalendarViewModel> EditCalendarEvent(int registrationId, UserCalendarEventEditModel model);
}