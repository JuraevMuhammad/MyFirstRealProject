using Domain.DTOs.User;
using Domain.Response;

namespace Infrastructure.Interfaces;

public interface IUserService
{
    Task<Response<string>> Register(CreatedUser dto);
    Task<string> Login(LoginUser dto);
}