using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;

public class RentalController(IRentalService service) : ControllerBase
{
    [HttpGet("rental-page")]
    public IActionResult GetRentalPage(BaseFilter filter)
    {
        var res = service.GetRentalPagination(filter);
        return StatusCode(res.StatusCode, res);
    }
}