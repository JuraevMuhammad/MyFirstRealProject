using System.Net;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.ComTypes;
using Domain.DTOs.Car;
using Domain.Entities;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class CarService(ApplicationDbContext context, IFileStorageService service) : ICarService
{
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
            res.ImagePath = await service.SaveFileAsync(dto.ImagePath, "images");
        
        context.Cars.Add(res);
        await context.SaveChangesAsync();

        return "Created Car";
    }

    #endregion

    #region GetAllCars

    public PaginationResponse<List<GetCar>> GetAllCars(CarFilter filter)
    {
        var res = context.Cars.AsQueryable();

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
        var res = context.Cars.FirstOrDefault(x => x.Id == id);
        if (res == null)
            return "not found";
        res.ImagePath = await service.SaveFileAsync(car.ImagePath!, "images");
        res.DailyPrice = (decimal)car.DailyPrice!;
        res.Brand = car.Brand ?? res.Brand;
        res.Model = car.Model ?? res.Model;
        res.Year = car.Year ?? res.Year;
        res.IsAvailable = car.IsAvailable ?? res.IsAvailable;
        
        await context.SaveChangesAsync();
        return "Updated Car";
    }

    #endregion

    #region GetCar

    public Response<GetCar> GetCar(int id)
    {
        var res = context.Cars.FirstOrDefault(x => x.Id == id);

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
    
    
    
}