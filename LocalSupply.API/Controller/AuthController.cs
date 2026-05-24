using LocalSupply.API.DTO.Auth;
using LocalSupply.API.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LocalSupply.API.Controller;

[ApiController]
[Route("api/auth")]
public class AuthController:ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO dto)
    {
        var response = await _authService.Register(dto);
        if (!response.Success) return BadRequest(response);
        return StatusCode(201,response);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        var response =await _authService.Login(dto);
        if (!response.Success) return Unauthorized(response);
        return Ok(response);
    }
}