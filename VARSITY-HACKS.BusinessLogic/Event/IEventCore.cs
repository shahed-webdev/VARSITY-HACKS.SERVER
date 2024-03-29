﻿using VARSITY_HACKS.DATA;
using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.BusinessLogic;

public interface IEventCore
{
    Task<ResponseModel<List<UserCalendarViewModel>>> AddAsync(string userName, UserEventAddModel model);
    Task<ResponseModel<List<UserCalendarViewModel>>> EditAsync(string userName, UserEventEditModel model);
    Task<ResponseModel<bool>> IsEventConflictingAsync(string userName, UserEventAddModel model);
    Task<ResponseModel<bool>> IsEventEditConflictingAsync(string userName, UserEventEditModel model);
    Task<ResponseModel<List<UserEventViewModel>>> GetEventsAsync(string userName, EventType type);
    Task<ResponseModel<EventTypeWiseEventViewModel>> GetTypeWiseEventsAsync(string userName);
    Task<ResponseModel<List<UserCalendarViewModel>>> GetCalendarEventsAsync(string userName);
    Task<ResponseModel> DeleteAsync(string userName, int userEventId);
    Task<ResponseModel> DeleteCalendarEventAsync(string userName, int calendarEventId);
    Task<ResponseModel<UserCalendarViewModel>> EditCalendarEventAsync(string userName, UserCalendarEventEditModel model);
}