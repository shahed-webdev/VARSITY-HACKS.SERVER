namespace VARSITY_HACKS.ViewModel;

public class UserCalendarViewModel
{
    public int UserCalendarEventId { get; set; }
    public int UserEventId { get; set; }
    public string EventName { get; set; } = null!;
    public string? BackgroundColor { get; set; }
    public string EventType { get; set; } = null!;
    public string? SubTitle { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public int DurationMinute { get; set; }
    public string Difficulty { get; set; } = null!;
    public string Priority { get; set; } = null!;
    public bool IsSimultaneous { get; set; }
    public bool IsSuggested { get; set; }
}