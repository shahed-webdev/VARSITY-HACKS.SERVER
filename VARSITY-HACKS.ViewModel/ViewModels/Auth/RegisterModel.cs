﻿using System.ComponentModel.DataAnnotations;

namespace VARSITY_HACKS.ViewModel;

public class RegisterModel
{
    public string Name { get; set; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}