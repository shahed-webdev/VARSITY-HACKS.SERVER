﻿using Itenso.TimePeriod;
using VARSITY_HACKS.DATA;
using VARSITY_HACKS.Repository;
using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.BusinessLogic;

public class SuggestedStudy :ISuggestedStudy
{
    private readonly DateTime _startDate;
    private readonly DateTime _endDate;
    private readonly DayOfWeek[] _days;
    private List<DateTime> WeekendDates { get; }
    private List<DateTime> EventDates { get; }


    public SuggestedStudy(DateTime startDate, DateTime endDate, DayOfWeek[] days)
    {

        _startDate = startDate;
        _endDate = endDate;        
        _days = days;

        WeekendDates = new List<DateTime>();
        EventDates = new List<DateTime>();

        GetWeekDates();


    }
    public void AddSuggestedStudy(int registrationId,int userEventId, PersonalityType type,DifficultyLevel difficulty, IUnitOfWork db)
    {
        var calendarDates = db.UserEvent.CalendarList(registrationId, _startDate, _endDate);

        var timePeriods = new TimePeriodCollection();

        foreach (var calendarDate in calendarDates)
        {
            timePeriods.Add(new TimeRange(calendarDate.StartDateTime, calendarDate.EndDateTime));
        }



        List<UserSuggestedEventAddModel> suggestedEvents = new List<UserSuggestedEventAddModel>();
        //add event date suggested Study
      var eventDateEvents =  GetSuggestedStudyEvents(registrationId, userEventId, difficulty, EventDates, timePeriods,
            SuggestedStudyRuleClass.WeekdayStarTime(type), SuggestedStudyRuleClass.GetEventDateDuration(type, difficulty), SuggestedStudyRuleClass.WeekdayDurationMinute(type));

        //add Weekend suggested Study
        var weekendEvents = GetSuggestedStudyEvents(registrationId, userEventId, difficulty, WeekendDates, timePeriods,
            SuggestedStudyRuleClass.WeekendStarTime(type), SuggestedStudyRuleClass.GetWeekendDuration(type, difficulty), SuggestedStudyRuleClass.WeekendDurationMinute(type));

        suggestedEvents.AddRange(eventDateEvents);
       suggestedEvents.AddRange(weekendEvents);
       
        db.UserEvent.AddSuggestedEvents(suggestedEvents);

        //add Night before event date suggested Study
        AddSuggestedStudyNightBeforeEventDate(registrationId, userEventId, type, difficulty, db);



    }


    public void AddSuggestedStudyNightBeforeEventDate(int registrationId, int userEventId, PersonalityType type, DifficultyLevel difficulty, IUnitOfWork db)
    {
       
        var calendarDates = db.UserEvent.CalendarList(registrationId, _startDate, _endDate);

        var timePeriods = new TimePeriodCollection();

        foreach (var calendarDate in calendarDates)
        {
            timePeriods.Add(new TimeRange(calendarDate.StartDateTime, calendarDate.EndDateTime));
        }
        var nightBeforeEventDates = EventDates.Select(d => d.AddDays(-1)).ToList();

        var nightBeforeEventDateEvents = GetSuggestedStudyEvents(registrationId, userEventId, difficulty, nightBeforeEventDates, timePeriods,
              SuggestedStudyRuleClass.WeekdayStarTime(type), SuggestedStudyRuleClass.GetEventDateDuration(type, difficulty), SuggestedStudyRuleClass.WeekdayDurationMinute(type));

        db.UserEvent.AddSuggestedEvents(nightBeforeEventDateEvents);
    }

        private void GetWeekDates()
    {

        for (var date = _startDate.Date; date.Date <= _endDate.Date; date = date.AddDays(1))
        {
            if (_days.Any(d => d == date.DayOfWeek))
            {
                EventDates.Add(date);
            }

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                WeekendDates.Add(date);
            }
        }
    }
    private IEnumerable<UserSuggestedEventAddModel> GetSuggestedStudyEvents(int registrationId, int userEventId, DifficultyLevel difficulty, List<DateTime> dates, TimePeriodCollection timePeriods, TimeSpan starTime, int suggestedStudyDuration, int totalDurationMinute)
    {
        var suggestedEvents = new List<UserSuggestedEventAddModel>();

        var eventBreak = SuggestedStudyRuleClass.EventBreakDuration;

        
        foreach (var date in dates)
        {

            var starDateTime = date.Date + starTime;
            var endDateTime = starDateTime.AddMinutes(totalDurationMinute);
            var limits = new CalendarTimeRange(starDateTime, endDateTime);

            var gapCalculator = new TimeGapCalculator<TimeRange>();
            var calcCaps = gapCalculator.GetGaps(timePeriods, limits);
            calcCaps.SortByDuration(ListSortDirection.Descending);

            if (calcCaps.Count > 0)
            {
                var maxGap = calcCaps.FirstOrDefault();
                var gapDuration =Convert.ToInt32( maxGap?.Duration.TotalMinutes);
                if (gapDuration >= suggestedStudyDuration)
                {
                    var initialBreak = gapDuration - suggestedStudyDuration >= eventBreak ? eventBreak : 0;
                    var startTime = maxGap?.Start == starDateTime ? maxGap.Start.TimeOfDay : maxGap?.Start.AddMinutes(initialBreak).TimeOfDay;
                    suggestedEvents.Add(new UserSuggestedEventAddModel
                    {
                        UserEventId = userEventId,
                        RegistrationId = registrationId,
                        EventDate = date,
                        StartTime = startTime.GetValueOrDefault(),
                        DurationMinute = suggestedStudyDuration,
                        Difficulty = difficulty
                    });
                }

            }
        }

        return suggestedEvents;
    }
}