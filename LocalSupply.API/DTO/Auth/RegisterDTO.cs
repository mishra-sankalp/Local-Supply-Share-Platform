using System.ComponentModel.DataAnnotations;

namespace LocalSupply.API.DTO.Auth;

public class RegisterDTO
{
    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
    public string FirstName { get; set; }
    
    public string? MiddleName { get; set; }
    
    public string LastName { get; set; }
    
    [Required]
    [StringLength(5)]
    [RegularExpression(@"^\+?\d+$", ErrorMessage = "Invalid country code.")]
    public string CountryCode { get; set; } = "91";
    [Required]
    [Phone] 
    [StringLength(15, MinimumLength = 7)]
    public string PhoneNumber { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
    public string Password { get; set; }

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
    public string FcmToken { get; set; }
}