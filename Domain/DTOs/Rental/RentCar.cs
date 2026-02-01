using Domain.Enums;

namespace Domain.DTOs.Rental;

public class RentCar
{
    public int UserId { get; set; }
    public int CarId { get; set; }
    public RentalStatus Status { get; set; }
}