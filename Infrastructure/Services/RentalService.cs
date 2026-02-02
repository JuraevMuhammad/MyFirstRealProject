using System.Net;
using Domain.DTOs.Rental;
using Domain.Entities;
using Domain.Enums;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Email;
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

    #region RentCar

    public async Task<Response<string>> RentCar(RentCar rent)
    {
        var resCar = _context.Cars.AsQueryable();
        var resUser = _context.Users.AsQueryable();
        
        var car = resCar.FirstOrDefault(x => x.Id == rent.CarId);
        var user = resUser.FirstOrDefault(x => x.Id == rent.UserId);
        
        if (car == null || user == null)
            return new Response<string>(HttpStatusCode.NotFound, "Car or User not found");

        if (car.IsAvailable)
            return new Response<string>(HttpStatusCode.BadRequest, "Car is not available");
            
        var rental = new Rental()
        {
            CarId = rent.CarId,
            UserId = rent.UserId,
            StartDate = DateTime.UtcNow,
            Status = rent.Status,
            TotalPrice = car.DailyPrice
        };
        
        car.IsAvailable = true;

        _context.Rentals.Add(rental);
        
        await _context.SaveChangesAsync();

        await _mail.RentalAsync(user, car, rental);
        
        return new Response<string>(HttpStatusCode.Created, "rental");
    }


    #endregion
    
    public async Task<Response<string>> ReturnCar(int id, ReturnCar dto)
    {
        var returnRental = _context.Rentals.AsQueryable();
        var cars = _context.Cars.AsQueryable();
        var users = _context.Users.AsQueryable();
        

        var rent = returnRental.FirstOrDefault(x => x.Id == id);
        if (rent == null)
            return new Response<string>(HttpStatusCode.NotFound, "not found");

        var car = cars.First(x => x.Id == rent.CarId);
        var user = users.First(x => x.Id == rent.UserId);
            
        rent.EndDate = DateTime.UtcNow;
        car.IsAvailable = false;
        rent.Status = RentalStatus.Canceled;

        if ((DateTime.UtcNow.Date - rent.StartDate.Date).Days > 1)
        {
            rent.TotalPrice = car.DailyPrice * (DateTime.UtcNow.Date - rent.StartDate.Date).Days;
            Console.WriteLine("TotalPrice: " + rent.TotalPrice);
        }

        if (rent.TotalPrice < car.DailyPrice)
        {
            rent.TotalPrice = car.DailyPrice;
            Console.WriteLine("TotalPrice: " + rent.TotalPrice);
        }

        await _context.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "Return Car");
    }

    public PaginationResponse<List<GetRental>> GetSortByRentalDate(RentalFilter filter)
    {
        var res = _context.Rentals.AsQueryable();
        
        if(filter.Status != null)
            res = res.Where(x => x.Status == filter.Status.Value);
        if (filter.StartDate != null)
            res = res.Where(x => x.EndDate >= filter.StartDate);
        if(filter.EndDate != null)
            res = res.Where(x => x.StartDate <= filter.EndDate);
        
        var totalRecord = res.Count();
        
        var result = res.Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize).ToList();

        var get = _logic.GetRentals(result);
        
        return new PaginationResponse<List<GetRental>>(filter.PageNumber, filter.PageSize, totalRecord, get);
    }
}