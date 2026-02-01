using Domain.DTOs.Rental;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;

[ApiController]
[Route("api/rental[controller]")]
public class RentalController(IRentalService service) : ControllerBase
{
    [HttpGet("page")]
    public IActionResult GetRentalPage([FromQuery]BaseFilter filter)
    {
        var res = service.GetRentalPagination(filter);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPost("car")]
    public async Task<IActionResult> RentCar([FromQuery]RentCar dto)
    {
        var res = await service.RentCar(dto);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPut("return-car")]
    public async Task<IActionResult> ReturnCar(int id, [FromQuery] ReturnCar dto)
    {
        var res = await service.ReturnCar(id, dto);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("filter")]
    public IActionResult GetRentalFilter([FromQuery] RentalFilter filter)
    {
        var res = service.GetSortByRentalDate(filter);
        return StatusCode(res.StatusCode, res);
    }
}