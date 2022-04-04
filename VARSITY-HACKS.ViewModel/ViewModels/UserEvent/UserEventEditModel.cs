using VARSITY_HACKS.DATA;

namespace VARSITY_HACKS.ViewModel;

public class UserEventEditModel
{
    public int UserEventId { get; set; }
    public string EventName { get; set; } = null!;
    public string? BackgroundColor { get; set; }
    public EventType EventType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string StartTime { get; set; } = null!;
    public int DurationMinute { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public PriorityLevel Priority { get; set; }
    public bool IsSimultaneous { get; set; }
    public DayOfWeek[] Days { get; set; } = null!;
}