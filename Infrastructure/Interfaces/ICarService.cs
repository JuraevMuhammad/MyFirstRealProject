using Domain.DTOs.Car;
using Domain.Filters;
using Domain.Response;

namespace Infrastructure.Interfaces;

public interface ICarService
{
    Task<string> CreatedCar(CreatedCar dto);
    PaginationResponse<List<GetCar>> GetAllCars(CarFilter filter);
    Task<string> UpdateCar(int id, UpdateCar car);
    Response<GetCar> GetCar(int id);
    
}