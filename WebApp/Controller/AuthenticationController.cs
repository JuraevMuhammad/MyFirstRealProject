using Domain.DTOs.User;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;

[ApiController]
[Route("api/auth")]
public class AuthenticationController(IUserService service) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(CreatedUser dto)
    {
        var res = await service.Register(dto);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUser dto)
    {
        var token = await service.Login(dto);

        return Ok(new { token });
    }
}