using LocalSupply.API.DTO.Auth;
using LocalSupply.API.Models.Common;

namespace LocalSupply.API.Services;
//interface is nothing just a contract or promise that anyone
// who implements it... will contain all those things inside the interface
public interface IAuthService
{
    Task<ApiResult<object>> Register(RegisterDTO dto);
    Task<ApiResult<object>> Login(LoginDTO dto);
}