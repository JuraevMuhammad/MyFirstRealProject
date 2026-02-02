using System.Net;
using Domain.DTOs.Car;
using Domain.DTOs.Rental;
using Domain.DTOs.User;
using Domain.Entities;
using Domain.Enums;
using Domain.Response;
using Infrastructure.Data;

namespace Infrastructure.Logic;

public class Logic(ApplicationDbContext context) : ILogic
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

    #region LogicEmail

    public EmailStatus ValidatorEmail(string email)
    {
        email = email.Trim();
        var res = context.Users.Any(x => x.Email == email);
        if (res)
            return EmailStatus.AlreadyExists;
        if (email.EndsWith("@mail.ru") || email.EndsWith(".com") && email.Contains('@') && email.Contains("mail"))
            return EmailStatus.Available;
        return EmailStatus.NotAllowed;
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
                EndDate = x.EndDate
            }
        ).ToList();
        
        return res;

    }

    #endregion

    #region LogicGetUser

    public List<GetUser> GetUsers(List<User> users)
    {
        return users.Select(x => new GetUser()
        {
            Id = x.Id,
            FullName = x.FullName,
            ProfileImage = x.ProfileImage,
            Role = x.Role,
            CreatedAt = x.CreatedAt
        }).ToList();
    }

    #endregion
}