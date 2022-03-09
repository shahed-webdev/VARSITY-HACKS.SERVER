namespace VARSITY_HACKS.DATA;

public class Registration
{
    public Registration()
    {
        Events = new HashSet<UserEvent>();
        CalendarEvents = new HashSet<UserCalendarEvent>();
    }
    public int RegistrationId { get; set; }
    public string UserName { get; set; } = null!;
    public bool Validation { get; set; }
    public PersonalityType Personality { get; set; }
    public UserMode Mode { get; set; }
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? UniversityName { get; set; }
    public string? Subject { get; set; }
    public string? SocialMediaLink { get; set; }
    public byte[]? Image { get; set; }
    public DateTime InsertDateUtc { get; set; }

    public virtual ICollection<UserEvent> Events { get; set; }
    public virtual ICollection<UserCalendarEvent> CalendarEvents { get; set; }
}