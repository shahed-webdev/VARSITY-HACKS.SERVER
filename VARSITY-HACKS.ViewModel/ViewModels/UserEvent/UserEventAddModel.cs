using VARSITY_HACKS.DATA;

namespace VARSITY_HACKS.ViewModel;

public class UserEventAddModel
{
    public string EventName { get; set; } = null!;
    public EventType EventType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public int DurationMinute { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public PriorityLevel Priority { get; set; }
    public bool IsSimultaneous { get; set; }
    public DayOfWeek[] Days { get; set; } = null!;
}