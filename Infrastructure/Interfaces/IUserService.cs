using Domain.DTOs.User;
using Domain.Filters;
using Domain.Response;

namespace Infrastructure.Interfaces;

public interface IUserService
{
    Task<Response<string>> Register(CreatedUser dto);
    Task<string> Login(LoginUser dto);

    PaginationResponse<List<GetUser>> GetPaginationUsers(UserFilter filter);
    Response<GetUser> GetUserById(int userId);
    
    Task<Response<string>> UpdateUser(int id, UpdateUser dto);
    Task<Response<string>> UpdatePasswordUser(int id, UpdatePasswordUser dto);
}