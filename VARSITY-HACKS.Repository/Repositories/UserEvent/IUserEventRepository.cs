using VARSITY_HACKS.DATA;
using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.Repository;

public interface IUserEventRepository
{
    ResponseModel<List<UserCalendarViewModel>> Add(int registrationId, UserEventAddModel model);
    ResponseModel<UserEventViewModel> Get(int id);
    bool IsExistName(int registrationId, string name);
    bool IsExistName(int registrationId, string name, int updateId);
    bool IsNull(int id);
    List<UserEventViewModel> List(int registrationId, EventType type);
    List<UserCalendarViewModel> CalendarList(int registrationId);
}