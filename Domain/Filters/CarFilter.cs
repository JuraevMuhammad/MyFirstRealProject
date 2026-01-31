namespace Domain.Filters;

public class CarFilter : BaseFilter
{
    public string? Brand { get; set; }
    public decimal? MaxPrice { get; set; }
    public decimal? MinPrice { get; set; }
}