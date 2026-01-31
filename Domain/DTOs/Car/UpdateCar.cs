
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.Car;

public class UpdateCar
{
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int? Year { get; set; }
    public decimal? DailyPrice { get; set; }
    public IFormFile? ImagePath { get; set; }
    public bool? IsAvailable { get; set; }
}