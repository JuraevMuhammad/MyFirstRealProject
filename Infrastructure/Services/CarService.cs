using System.Net;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.ComTypes;
using Domain.DTOs.Car;
using Domain.Entities;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Logic;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services;

public class CarService : ICarService
{
    #region Constructor

    private readonly ILogic _logic;
    private readonly IFileStorageService _service;
    private readonly ApplicationDbContext _context;
    public CarService(ILogic logic,
        IFileStorageService service,
        ApplicationDbContext context) 
    {
        _logic = logic;
        _service = service;
        _context = context;
    }

    #endregion
    
    #region CreatedCar

    public async Task<string> CreatedCar(CreatedCar dto)
    {
        var res = new Car()
        {
            Brand = dto.Brand,
            Model = dto.Model,
            Year = dto.Year,
            DailyPrice = dto.DailyPrice,
            IsAvailable = dto.IsAvailable,
        };
        

        if (dto.ImagePath != null)
            res.ImagePath = await _service.SaveFileAsync(dto.ImagePath, "images");
        
        _context.Cars.Add(res);
        await _context.SaveChangesAsync();

        return "Created Car";
    }

    #endregion

    #region GetAllCars

    public PaginationResponse<List<GetCar>> GetAllCars(CarFilter filter)
    {
        var res = _context.Cars.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Brand))
            res = res.Where(x => x.Brand.ToLower().Contains(filter.Brand.ToLower()));

        if (filter.MaxPrice.HasValue && filter.MinPrice.HasValue)
            res = res.Where(x => x.DailyPrice <= filter.MaxPrice && x.DailyPrice >= filter.MinPrice);

        var totalRecords = res.Count();
        
        var result = res.Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize).ToList();

        var gets = result.Select(x => new GetCar()
        {
            Id = x.Id,
            Brand = x.Brand,
            Model = x.Model,
            Year = x.Year,
            ImagePath = x.ImagePath ?? "not found",
            DailyPrice = x.DailyPrice,
            IsAvailable = x.IsAvailable,
            CreatedAt = x.CreatedAt
        }).ToList();
        
        return new PaginationResponse<List<GetCar>>(filter.PageNumber, filter.PageSize, totalRecords, gets);
    }

    #endregion

    #region UpdateCar

    public async Task<string> UpdateCar(int id, UpdateCar car)
    {
        var res = _context.Cars.FirstOrDefault(x => x.Id == id);
        if (res == null)
            return "not found";
        res.ImagePath = await _service.SaveFileAsync(car.ImagePath!, "images");
        res.DailyPrice = (decimal)car.DailyPrice!;
        res.Brand = car.Brand ?? res.Brand;
        res.Model = car.Model ?? res.Model;
        res.Year = car.Year ?? res.Year;
        res.IsAvailable = car.IsAvailable ?? res.IsAvailable;
        
        await _context.SaveChangesAsync();
        return "Updated Car";
    }

    #endregion

    #region GetCar

    public Response<GetCar> GetCar(int id)
    {
        var res = _context.Cars.FirstOrDefault(x => x.Id == id);

        var get = new GetCar()
        {
            Id = res!.Id,
            Brand = res.Brand,
            Model = res.Model,
            Year = res.Year,
            DailyPrice = res.DailyPrice,
            IsAvailable = res.IsAvailable,
            CreatedAt = res.CreatedAt
        };
        return new Response<GetCar>(get);
    }

    #endregion

    #region DeleteCar

    public async Task<Response<string>> DeleteCar(int id)
    {
        var car = _context.Cars.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (car == null)
            return new Response<string>(HttpStatusCode.NotFound, "not found");
        car.IsDeleted = true;

        await _context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK, "Deleted Car");
    }
    
    #endregion

    #region GetCarSearchByBrandAndModel

    public PaginationResponse<List<GetCar>> GetCarSearchByBrandAndModel(NameCarFilter filter)
    {
        var quary = _context.Cars.AsQueryable();
        
        if (!string.IsNullOrEmpty(filter.Brand))
            quary = quary.Where(x => x.Brand.ToLower()
                    .Contains(filter.Brand.ToLower()));
        
        if (!string.IsNullOrEmpty(filter.Model))
            quary = quary.Where(x => x.Model.ToLower()
                .Contains(filter.Model.ToLower()));
        
        var totalRecords = quary.Count();
        
        var res = quary.Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize).ToList();

        var getCar = _logic.GetCar(res);
        
        return new PaginationResponse<List<GetCar>>(filter.PageNumber, filter.PageSize, totalRecords, getCar);
    }
    
    #endregion

    #region GetCarOrderByCreatedAt

    public Response<List<GetCar>> GetCarOrderByCreatedAt()
    {
        var res = _context.Cars.OrderBy(x => x.CreatedAt).ToList();

        var getCars = _logic.GetCar(res);
        
        return new Response<List<GetCar>>(getCars);
    }

    #endregion
}