using Domain.Enums;

namespace Domain.DTOs.Rental;

public class ReturnCar
{
    public int UserId { get; set; }
    public int CarId { get; set; }
}