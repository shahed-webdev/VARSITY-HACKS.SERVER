namespace VARSITY_HACKS.DATA;

public class Registration
{
    //public Registration()
    //{
    //    Tasks = new HashSet<UserTask>();
    //    CalendarTaskList = new HashSet<UserTaskCalendar>();
    //}
    public int RegistrationId { get; set; }
    public string UserName { get; set; } = null!;
    public bool Validation { get; set; }
    public PersonalityType Personality { get; set; }
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? UniversityName { get; set; }
    public string? Subject { get; set; }
    public string? SocialMediaLink { get; set; }
    public byte[]? Image { get; set; }
    public DateTime InsertDateUtc { get; set; }

   // public virtual ICollection<UserTask> Tasks { get; set; }
   // public virtual ICollection<UserTaskCalendar> CalendarTaskList { get; set; }
}