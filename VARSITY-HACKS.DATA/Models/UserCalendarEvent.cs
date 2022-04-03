namespace VARSITY_HACKS.DATA;

public class UserCalendarEvent
{
    public int UserCalendarEventId { get; set; }
    public int UserEventId { get; set; }
    public int RegistrationId { get; set; }
    public string? SubTitle { get; set; }
    public DateTime EventDate { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public TimeSpan StartTime { get; set; }
    public int DurationMinute { get; set; }
    public TimeSpan EndTime { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public PriorityLevel Priority { get; set; }
    public bool IsSimultaneous { get; set; }
    public bool IsSuggested { get; set; }
    public DateTime InsertDateUtc { get; set; }
    public virtual UserEvent UserEvent { get; set; } = null!;
    public virtual Registration Registration { get; set; } = null!;
}