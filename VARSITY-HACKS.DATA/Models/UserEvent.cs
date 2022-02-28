namespace VARSITY_HACKS.DATA;

public class UserEvent
{
    public UserEvent()
    {
        Days = new HashSet<UserEventDay>();
        CalendarEvents = new HashSet<UserCalendarEvent>();
    }

    public int UserEventId { get; set; }
    public int RegistrationId { get; set; }
    public string EventName { get; set; } = null!;
    public EventType EventType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int DurationMinute { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public PriorityLevel Priority { get; set; }
    public bool IsSimultaneous { get; set; }
    public DateTime InsertDateUtc { get; set; }
    public virtual Registration Registration { get; set; } = null!;
    public virtual ICollection<UserEventDay> Days { get; set; }
    public virtual ICollection<UserCalendarEvent> CalendarEvents { get; set; }
}