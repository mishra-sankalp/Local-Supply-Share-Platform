using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FirebaseAdmin.Auth;
using LocalSupply.API.Data;
using LocalSupply.API.DTO.Auth;
using LocalSupply.API.Models.Common;
using LocalSupply.API.Models.DataModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LocalSupply.API.Services;

public class AuthService : IAuthService
{
    private readonly AppDBContext _context;
    private readonly IConfiguration _config;

    public AuthService(AppDBContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<ApiResult<object>> Register(RegisterDTO dto)
    {
        var isPhoneNumberExists = await _context.Users.AnyAsync(x => x.PhoneNumber == dto.PhoneNumber && x.CountryCode == dto.CountryCode);
        if (isPhoneNumberExists)
        {
            return ApiResult<object>.Fail("User already exists");
        }
        var hashedPass = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.Password);
        var data = new User
        {
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            CountryCode = dto.CountryCode,
            PhoneNumber = dto.PhoneNumber,
            CreditBalance = 0,
            PasswordHash = hashedPass,
            FcmToken = dto.FcmToken
        };
        _context.Users.Add(data);
        await _context.SaveChangesAsync();
        return ApiResult<object>.Ok("User Registered Successfully");
    }

    public async Task<ApiResult<object>> Login(LoginDTO dto)
    {
        var user = await _context.Users.Where(x => x.PhoneNumber == dto.PhoneNumber && x.CountryCode == dto.CountryCode).FirstOrDefaultAsync();
        if (user != null)
        {
            var existingPass = user.PasswordHash;
            var isPassCorrect = BCrypt.Net.BCrypt.EnhancedVerify(dto.Password, existingPass);
            if (isPassCorrect)
            {
                var issuer = _config["Jwt:Issuer"];
                var audience = _config["Jwt:Audience"];
                var jwtSecret = _config["Jwt:Key"];
                var expiryDays = DateTime.Now.AddDays(Convert.ToInt32(_config["Jwt:ExpiryDays"]));
                var token = GenerateToken(user,jwtSecret,expiryDays,issuer,audience);
                object obj = new AuthResponseDTO
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    Token = token,
                    ExpiresAt = expiryDays
                };
                return ApiResult<object>.Ok(obj, "Login successful");
            }
        }
        return ApiResult<object>.Fail("Invalid Credentials!!");
    }

    private string GenerateToken(User user, string jwtSecret, DateTime? expireIn,string issuer,string audience)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),new (ClaimTypes.MobilePhone, user.PhoneNumber), new(ClaimTypes.Name, user.FirstName)
        };
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret)
            ),
            SecurityAlgorithms.HmacSha256Signature);
        var jwtToken = new JwtSecurityToken(
            issuer:issuer,
            audience:audience,
            claims: claims,
            signingCredentials:signingCredentials,
            notBefore:DateTime.UtcNow,
            expires:expireIn
            );
        string token =  new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return token;
    }
}