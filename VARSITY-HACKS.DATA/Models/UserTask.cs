namespace VARSITY_HACKS.DATA;

public class UserTask
{
    public UserTask()
    {
        Days = new HashSet<UserTaskDay>();
        CalendarTaskList = new HashSet<UserTaskCalendar>();
    }
    public int UserTaskId { get; set; }
    public int RegistrationId { get; set; }
    public string TaskName { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int DurationMinute { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public PriorityLevel Priority { get; set; }
    public DateTime InsertDateUtc { get; set; }
    public bool IsSimultaneous { get; set; }
    public virtual Registration Registration { get; set; } = null!;
    public virtual ICollection<UserTaskDay> Days { get; set; }
    public virtual ICollection<UserTaskCalendar> CalendarTaskList { get; set; }
}