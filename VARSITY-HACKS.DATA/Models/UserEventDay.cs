namespace VARSITY_HACKS.DATA;

public class UserEventDay
{
    public int UserEventDayId { get; set; }
    public int UserEventId { get; set; }
    public DayOfWeek Day { get; set; }
    public virtual UserEvent UserEvent { get; set; } = null!;
}