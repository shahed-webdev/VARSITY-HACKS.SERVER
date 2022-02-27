namespace VARSITY_HACKS.DATA;

public class UserTaskDay
{
    public int UserTaskDayId { get; set; }
    public int UserTaskId { get; set; }
    public DayOfWeek Day { get; set; }
    public virtual UserTask UserTask { get; set; } = null!;
}