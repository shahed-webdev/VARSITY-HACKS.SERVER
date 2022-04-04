namespace VARSITY_HACKS.ViewModel;

public class UserCalendarEventEditModel
{
    public int UserCalendarEventId { get; set; }
    public string? SubTitle { get; set; }
    public DateTime EventDate { get; set; }
    public string StartTime { get; set; }
    public int DurationMinute { get; set; }
}