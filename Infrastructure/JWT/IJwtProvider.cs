using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(User user);
}