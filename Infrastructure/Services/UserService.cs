using System.Net;
using Domain.DTOs.User;
using Domain.Entities;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    #region Constructor

    private readonly IPasswordHasher _passwordHasher;
    private readonly ApplicationDbContext _dbContext;
    private readonly IFileStorageService _fileService;
    private readonly IJwtProvider _jwtProvider;

    public UserService(IPasswordHasher passwordHasher,
        ApplicationDbContext dbContext,
        IFileStorageService fileService,
        IJwtProvider jwtProvider)
    {
        _passwordHasher = passwordHasher;
        _dbContext = dbContext;
        _fileService = fileService;
        _jwtProvider = jwtProvider;
    }

    #endregion


    #region Register

    public async Task<Response<string>> Register(CreatedUser dto)
    {
        var hashedPassword = _passwordHasher.Generate(dto.Password);

        var created = new User()
        {
            FullName = dto.FullName,
            PasswordHash = hashedPassword,
            Email = dto.Email,
            Role = dto.Role,
            CarId = dto.CarId
        };
        
        if (dto.ProfileImage != null)
            created.ProfileImage = await _fileService.SaveFileAsync(dto.ProfileImage, "images");
        
        _dbContext.Users.Add(created);
        await _dbContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.Created, "Register user");
    }
    
    #endregion

    public async Task<string> Login(LoginUser dto)
    {
        var user = _dbContext.Users.FirstOrDefault(x => x.Email == dto.Email);

        if (user == null)
            return "not found";
        
        var result = _passwordHasher.Verify(dto.Password, user.PasswordHash);
        
        if(!result) throw new Exception("invalid password");
        
        var token = _jwtProvider.GenerateToken(user);
        
        return token;
    }
}