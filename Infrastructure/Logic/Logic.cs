using System.Net;
using Domain.DTOs.Car;
using Domain.DTOs.Rental;
using Domain.Entities;
using Domain.Response;

namespace Infrastructure.Logic;

public class Logic : ILogic
{
    #region LogicPassword

    public bool ValidatorPassword(string password)
    {
        int p = 0, cnt = 0, sum = 0;
        foreach (var s in password)
        {
            p++;
            if (s >= 65 && s <= 90 || s>=97 && s<=122)
            {
                cnt++;
            }
            if (s >= 33 && s <=64)
            {
                sum++;        
            }
        }
        return p >= 8 && cnt >= 1 && sum >=1;
    }

    #endregion

    #region LogicGetCar

    public List<GetCar> GetCar(List<Car> cars)
    {
        var res = cars.Select(x => new GetCar()
            {
                Id = x.Id,
                Brand = x.Brand,
                Model = x.Model,
                Year = x.Year,
                DailyPrice = x.DailyPrice,
                ImagePath = x.ImagePath!,
                IsAvailable = x.IsAvailable,
                CreatedAt = x.CreatedAt,
            }
        ).ToList();
        
        return res;
    }

    #endregion

    #region LogicGetRental

    public List<GetRental> GetRentals(List<Rental> rentals)
    {
        var res = rentals.Select(x => new GetRental()
            {
                Id = x.Id,
                CarId = x.CarId,
                UserId = x.UserId,
                TotalPrice = x.TotalPrice,
                Status = x.Status,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
            }
        ).ToList();
        
        return res;
    }

    #endregion
}