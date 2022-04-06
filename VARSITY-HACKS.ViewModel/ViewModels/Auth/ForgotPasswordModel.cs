using System.ComponentModel.DataAnnotations;

namespace VARSITY_HACKS.ViewModel;

public class ForgotPasswordModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    public string ResetPasswordUrl { get; set; } = null!;
    
}