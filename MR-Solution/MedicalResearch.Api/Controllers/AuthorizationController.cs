using MedicalResearch.Api.DTO;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalResearch.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthorizationController(ITokensService tokensService, IUserService userService) :ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var user = await userService.GetUserByEmailAsync(loginDTO.Email);
        if (user == null)
        {
            return NotFound("User not found");
        }
        var isValid = await userService.ValidateUserAsync(loginDTO.Email, loginDTO.Password);
        if (!isValid)
        {
            return Unauthorized("Invalid email or password");
        }

        var (token, expiration) = tokensService.GenerateAccessToken(user);

        return Ok(new
        {
            token,
            expiration,
        });
    }
}
