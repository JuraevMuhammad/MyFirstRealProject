using Domain.DTOs.Car;
using Domain.DTOs.Rental;
using Domain.Entities;
using Domain.Response;

namespace Infrastructure.Logic;

public interface ILogic
{
    bool ValidatorPassword(string password);
    List<GetCar> GetCar(List<Car> cars);
    List<GetRental> GetRentals(List<Rental> cars);
}