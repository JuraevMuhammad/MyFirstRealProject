using Domain.DTOs.Rental;
using Domain.Filters;
using Domain.Response;

namespace Infrastructure.Interfaces;

public interface IRentalService
{
    PaginationResponse<List<GetRental>> GetRentalPagination(BaseFilter filter);
    Task<Response<string>> RentCar(RentCar rent);
    Task<Response<string>> ReturnCar(int id, ReturnCar dto);
    PaginationResponse<List<GetRental>> GetSortByRentalDate(RentalFilter filter);

}