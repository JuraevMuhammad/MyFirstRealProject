using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Enums;

namespace WebApp.Controller;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    #region Constructor

    private readonly ApplicationDbContext _context;
    private readonly ILogic _logic;

    public StatisticsController(
        ApplicationDbContext context,
        ILogic logic)
    {
        _context = context;
        _logic = logic;
    }

    #endregion

    [HttpGet("total-users")]
    public IActionResult TotalUsers()
    {
        var userCount = _context.Users.Count(x => x.IsDeleted == false);
        return Ok(new { userCount });
    }

    [HttpGet("total-cars")]
    public IActionResult TotalCars()
    {
        var carCount = _context.Cars.Count(x => x.IsDeleted == false);
        return Ok(new { carCount });
    }

    [HttpGet("total-rentals")]
    public IActionResult TotalRentals()
    {
        var totalRentals = _context.Rentals.Count(x => x.IsDeleted == false);
        return Ok(new { totalRentals });
    }

    [HttpGet("active-rentas")]
    public IActionResult TotalActiveRentas()
    {
        var activeRentals = _context.Rentals.Count(x => x.Status == RentalStatus.Active);
        return Ok(new { activeRentals });
    }

    [HttpGet("revenue")]
    public IActionResult TotalRevenue()
    {
        var revenue = _context.Rentals.Where(x => x.Status == RentalStatus.Canceled).Sum(x => x.TotalPrice);
        return Ok(new { revenue });
    }
}