using Domain.DTOs.Car;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;

[ApiController]
[Route("[controller]")]
public class CarController(ICarService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Created([FromForm] CreatedCar dto)
    {
        var res = await service.CreatedCar(dto);
        return Ok(res);
    }

    [HttpGet("[Action]")]
    public IActionResult GetAll([FromQuery] CarFilter filter)
    {
        var res = service.GetAllCars(filter);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCar(int id,[FromForm] UpdateCar car)
    {
        var res = await service.UpdateCar(id, car);
        return Ok(res);
    }

    [HttpGet("Id")]
    public IActionResult GetById(int id)
    {
        var res = service.GetCar(id);
        return StatusCode(res.StatusCode, res);
    }
}