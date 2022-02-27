using VARSITY_HACKS.DATA;

namespace VARSITY_HACKS.ViewModel;

public class RegistrationEditModel
{
    public int RegistrationId { get; set; }
    public PersonalityType Personality { get; set; }
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? UniversityName { get; set; }
    public string? Subject { get; set; }
    public string? SocialMediaLink { get; set; }
    public byte[]? Image { get; set; }

}