using Domain.Entities;

namespace Infrastructure.JWT;

public interface IJwtProvider
{
    string GenerateToken(User user);
}