using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;

[ApiController]
[Route("api/user/[controller]")]
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
}