namespace LocalSupply.API.DTO.Auth;

public class AuthResponseDTO
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}