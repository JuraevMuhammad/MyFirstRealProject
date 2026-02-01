using Domain.Enums;

namespace Domain.Entities;

public class Rental
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CarId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public RentalStatus Status { get; set; }
    public bool IsDeleted { get; set; } 
    
    public Car? Car { get; set; }
    public User? User { get; set; }
}