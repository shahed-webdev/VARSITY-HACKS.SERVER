using Itenso.TimePeriod;
using VARSITY_HACKS.DATA;
using VARSITY_HACKS.Repository;
using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.BusinessLogic;

public class SuggestedStudyNightOwl:ISuggestedStudy
{
    private readonly DateTime _startDate;
    private readonly DateTime _endDate;
    private readonly DifficultyLevel _difficulty;
    private readonly DayOfWeek[] _days;
    private List<DateTime> WeekendDates { get; }
    private List<DateTime> EventDates { get; }
    private TimeSpan WeekdayStarTime { get; }
    private int WeekdayDurationMinute { get; }
    private TimeSpan WeekendStarTime { get; }
    private int WeekendDurationMinute { get; }
    private int EventBreak { get; }
    public SuggestedStudyNightOwl(DateTime startDate, DateTime endDate, DifficultyLevel difficulty, DayOfWeek[] days)
    {
        EventBreak = 30;
        _startDate = startDate;
        _endDate = endDate;
        _difficulty = difficulty;
        _days = days;

        WeekendDates = new List<DateTime>();
        EventDates = new List<DateTime>();

        GetWeekDates(_startDate, _endDate);
        WeekdayStarTime = TimeSpan.Parse("18:00");
        WeekdayDurationMinute = 540;
        WeekendStarTime = TimeSpan.Parse("17:00");
        WeekendDurationMinute = 210;
    }
    public void AddSuggestedStudy(int registrationId, int userEventId, DifficultyLevel difficulty, IUnitOfWork db)
    {
        var calendarDates = db.UserEvent.CalendarList(registrationId, _startDate, _endDate);

        var timePeriods = new TimePeriodCollection();

        foreach (var calendarDate in calendarDates)
        {
            timePeriods.Add(new TimeRange(calendarDate.StartDateTime, calendarDate.EndDateTime));
        }



        List<UserSuggestedEventAddModel> suggestedEvents = new List<UserSuggestedEventAddModel>();
        //add event date suggested Study
        foreach (DateTime date in EventDates)
        {

            var starTime = date.Date + WeekdayStarTime;
            var endTime = starTime.AddMinutes(WeekdayDurationMinute);
            var limits = new CalendarTimeRange(starTime, endTime);

            TimeGapCalculator<TimeRange> gapCalculator = new TimeGapCalculator<TimeRange>();
            ITimePeriodCollection calcCaps = gapCalculator.GetGaps(timePeriods, limits);
            calcCaps.SortByDuration(ListSortDirection.Descending);

            if (calcCaps.Count > 0)
            {
                var maxGap = calcCaps.FirstOrDefault();
                var suggestedStudyDuration = GetEventDateSuggestedStudyDuration();
                if (maxGap?.Duration.Minutes >= (EventBreak + suggestedStudyDuration))
                {
                    suggestedEvents.Add(new UserSuggestedEventAddModel
                    {
                        UserEventId = userEventId,
                        RegistrationId = registrationId,
                        EventDate = date,
                        StartTime = maxGap.Start.AddMinutes(EventBreak + suggestedStudyDuration).TimeOfDay,
                        DurationMinute = suggestedStudyDuration,
                        Difficulty = difficulty
                    });
                }
            }



        }
        //add Weekend suggested Study
        foreach (DateTime date in WeekendDates)
        {

            var starTime = date.Date + WeekendStarTime;
            var endTime = starTime.AddMinutes(WeekendDurationMinute);
            var limits = new CalendarTimeRange(starTime, endTime);

            TimeGapCalculator<TimeRange> gapCalculator = new TimeGapCalculator<TimeRange>();
            ITimePeriodCollection calcCaps = gapCalculator.GetGaps(timePeriods, limits);
            calcCaps.SortByDuration(ListSortDirection.Descending);

            if (calcCaps.Count > 0)
            {
                var maxGap = calcCaps.FirstOrDefault();
                var suggestedStudyDuration = GetWeekendSuggestedStudyDuration();
                if (maxGap?.Duration.Minutes >= (EventBreak + suggestedStudyDuration))
                {
                    suggestedEvents.Add(new UserSuggestedEventAddModel
                    {
                        UserEventId = userEventId,
                        RegistrationId = registrationId,
                        EventDate = date,
                        StartTime = maxGap.Start.AddMinutes(EventBreak + suggestedStudyDuration).TimeOfDay,
                        DurationMinute = suggestedStudyDuration,
                        Difficulty = difficulty
                    });
                }
            }



        }

        db.UserEvent.AddSuggestedEvents(suggestedEvents);
    }

    private void GetWeekDates(DateTime startDate, DateTime endDate)
    {

        for (var date = startDate.Date; date.Date <= endDate.Date; date = date.AddDays(1))
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

    private int GetEventDateSuggestedStudyDuration()
    {
        switch (_difficulty)
        {
            case DifficultyLevel.Low:
                return 60;
            case DifficultyLevel.Moderate:
                return 90;
            default:
                return 120;
        }
    }

    private int GetWeekendSuggestedStudyDuration()
    {
        switch (_difficulty)
        {
            case DifficultyLevel.Low:
                return 90;
            case DifficultyLevel.Moderate:
                return 120;
            default:
                return 150;
        }
    }
}