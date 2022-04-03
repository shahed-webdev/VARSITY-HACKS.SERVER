using VARSITY_HACKS.DATA;

namespace VARSITY_HACKS.ViewModel;

public class UserSuggestedEventAddModel
{
    public int UserEventId { get; set; }
    public int RegistrationId { get; set; }
    public DateTime EventDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public int DurationMinute { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public PriorityLevel Priority { get; } = PriorityLevel.Low;
    public bool IsSimultaneous { get; } = true;
    public bool IsSuggested { get;  } = true;
}