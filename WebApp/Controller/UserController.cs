using Domain.DTOs.User;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService service) : ControllerBase
{
    [HttpGet("pagination")]
    public IActionResult GetPaginationUsers([FromQuery]UserFilter filter)
    {
        var res = service.GetPaginationUsers(filter);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        var res = service.GetUserById(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser([FromQuery] int id, UpdateUser dto)
    {
        var res = await service.UpdateUser(id, dto);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPut("update-password")]
    public async Task<IActionResult> UpdatePasswordUser([FromQuery] int id, UpdatePasswordUser dto)
    {
        var res = await service.UpdatePasswordUser(id, dto);
        return StatusCode(res.StatusCode, res);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var res = await service.DeleteUser(id);
        return StatusCode(res.StatusCode, res);
    }
}