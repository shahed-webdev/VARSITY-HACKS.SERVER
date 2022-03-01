namespace VARSITY_HACKS.ViewModel;

public class UserEventViewModel
{
    public int UserEventId { get; set; }
    public string EventName { get; set; } = null!;
    public string? BackgroundColor { get; set; }
    public string EventType { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int DurationMinute { get; set; }
    public string Difficulty { get; set; } = null!;
    public string Priority { get; set; } = null!;
    public bool IsSimultaneous { get; set; }
    public DateTime InsertDateUtc { get; set; }
}