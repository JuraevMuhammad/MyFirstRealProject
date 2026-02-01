using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace Domain.DTOs.Car;

public class CreatedCar
{
    [Required]
    public required string Brand { get; set; }
    [Required]
    public required string Model { get; set; }
    public int Year { get; set; }
    public decimal DailyPrice { get; set; }
    public IFormFile? ImagePath { get; set; }
}
