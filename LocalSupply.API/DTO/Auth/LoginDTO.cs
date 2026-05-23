using System.ComponentModel.DataAnnotations;

namespace LocalSupply.API.DTO.Auth;

public class LoginDTO
{
    [Required]
    [StringLength(5)]
    [RegularExpression(@"^\+?\d+$", ErrorMessage = "Invalid country code.")]
    public string CountryCode { get; set; } = "+91";

    [Required]
    [Phone]
    [StringLength(15, MinimumLength = 7)]
    public string PhoneNumber { get; set; }

    [Required]
    // We don't necessarily need MinimumLength here. If they send a 3-character 
    // password, it just won't match the db and will fail login normally.
    // But we absolutely must put cap on  the maximum length to prevent DoS attacks 
    // against theee password hashing algorithm.
    [StringLength(100)] 
    public string Password { get; set; }
}