using Domain.DTOs.Car;
using Domain.DTOs.Rental;
using Domain.DTOs.User;
using Domain.Entities;
using Domain.Enums;
using Domain.Response;

namespace Infrastructure.Logic;

public interface ILogic
{
    bool ValidatorPassword(string password);
    EmailStatus ValidatorEmail(string email);
    List<GetCar> GetCar(List<Car> cars);
    List<GetRental> GetRentals(List<Rental> cars);
    List<GetUser> GetUsers(List<User> users);
}