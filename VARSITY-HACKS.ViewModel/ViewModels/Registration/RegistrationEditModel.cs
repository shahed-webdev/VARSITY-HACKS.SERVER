using Microsoft.AspNetCore.Http;

namespace VARSITY_HACKS.ViewModel;

public class RegistrationEditModel
{
    public int RegistrationId { get; set; }
    public string Personality { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? UniversityName { get; set; }
    public string? Subject { get; set; }
    public string? SocialMediaLink { get; set; }
    public byte[]? Image { get; set; }
}

//for image file
public class RegistrationEditModelWithFormFile : RegistrationEditModel
{
    public IFormFile? FormFile { get; set; }
}