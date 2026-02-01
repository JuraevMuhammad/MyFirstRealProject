using Domain.Enums;

namespace Domain.Filters;

public class RentalFilter : BaseFilter
{
    public RentalStatus? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}