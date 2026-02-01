using Domain.DTOs.Rental;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Logic;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Services;

public class RentalService : IRentalService
{
    #region Constructor

    private readonly ApplicationDbContext _context;
    private readonly ILogic _logic;
    
    public RentalService(
        ApplicationDbContext context,
        ILogic logic)
    {
        _context = context;
        _logic = logic;
    }

    #endregion

    [Authorize(Roles = "Admin")]
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
}