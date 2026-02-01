using System.Net;
using Domain.DTOs.Rental;
using Domain.Entities;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Logic;

namespace Infrastructure.Services;

public class RentalService : IRentalService
{
    #region Constructor

    private readonly ApplicationDbContext _context;
    private readonly ILogic _logic;
    private readonly ISandMail _mail;
    
    public RentalService(
        ApplicationDbContext context,
        ILogic logic,
        ISandMail mail)
    {
        _context = context;
        _logic = logic;
        _mail = mail;
    }

    #endregion
    
    
    #region GetRentalPagination

    public PaginationResponse<List<GetRental>> GetRentalPagination(BaseFilter filter)
    {
        var rentals = _context.Rentals.AsQueryable();
        
        var totalRecord = rentals.Count();
        
        var res = rentals.Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize).ToList();

        var result = _logic.GetRentals(res);
        
        return new PaginationResponse<List<GetRental>>(filter.PageNumber, filter.PageSize, totalRecord, result);
    }

    #endregion

    public async Task<Response<string>> RentCar(RentCar rent)
    {
        var resCar = _context.Cars.AsQueryable();
        var resUser = _context.Users.AsQueryable();
        
        var car = resCar.FirstOrDefault(x => x.Id == rent.CarId);
        var user = resUser.FirstOrDefault(x => x.Id == rent.UserId);
        
        if (car == null || user == null)
            return new Response<string>(HttpStatusCode.NotFound, "Car or User not found");

        var rental = new Rental()
        {
            CarId = rent.CarId,
            UserId = rent.UserId,
            StartDate = DateTime.UtcNow,
            Status = rent.Status,
            TotalPrice = car.DailyPrice
        };
        
        car.IsAvailable = true;
        user.CarId = rent.CarId;
        user.RentalId = rental.Id;

        _context.Rentals.Add(rental);
        
        await _context.SaveChangesAsync();

        await _mail.RentalAsync(user, car, rental);
        
        return new Response<string>(HttpStatusCode.Created, "rental");
    }
}