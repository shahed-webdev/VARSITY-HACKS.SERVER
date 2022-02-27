namespace VARSITY_HACKS.DATA;

public class UserTaskCalendar
{
    public int UserTaskCalendarId { get; set; }
    public int UserTaskId { get; set; }
    public int RegistrationId { get; set; }
    public string? SubTitle { get; set; }
    public DateTime TaskDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public DateTime InsertDateUtc { get; set; }
    public bool IsSuggested { get; set; }
    public virtual UserTask UserTask { get; set; } = null!;
    public virtual Registration Registration { get; set; } = null!;
}