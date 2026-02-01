using Domain.DTOs.Rental;
using Domain.Filters;
using Domain.Response;

namespace Infrastructure.Interfaces;

public interface IRentalService
{
    PaginationResponse<List<GetRental>> GetRentalPagination(BaseFilter filter);
    Task<Response<string>> RentCar(RentCar rent);

}