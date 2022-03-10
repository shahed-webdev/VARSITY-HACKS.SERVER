namespace VARSITY_HACKS.ViewModel;

public class EventTypeWiseEventViewModel
{
    public EventTypeWiseEventViewModel()
    {
        School = new List<UserEventViewModel>();
        Work = new List<UserEventViewModel>();
        Personal = new List<UserEventViewModel>();
    }

    public List<UserEventViewModel> School { get; set; }
    public List<UserEventViewModel> Work { get; set; }
    public List<UserEventViewModel> Personal { get; set; }
}