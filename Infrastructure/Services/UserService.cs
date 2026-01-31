using System.Net;
using Domain.DTOs.User;
using Domain.Entities;
using Domain.Filters;
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
    private readonly ISandMail _mail;

    public UserService(IPasswordHasher passwordHasher,
        ApplicationDbContext dbContext,
        IFileStorageService fileService,
        IJwtProvider jwtProvider,
        ISandMail mail)
    {
        _passwordHasher = passwordHasher;
        _dbContext = dbContext;
        _fileService = fileService;
        _jwtProvider = jwtProvider;
        _mail = mail;
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
        
        await _mail.SendAsync(created, dto.Password);
        
        await _dbContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.Created, "Register user");
    }
    
    #endregion

    #region Login

    public async Task<string> Login(LoginUser dto)
    {
        var user = _dbContext.Users.FirstOrDefault(x => x.Email == dto.Email);

        if (user == null)
            return "not found";
        
        var result = _passwordHasher.Verify(dto.Password, user.PasswordHash);
        
        if(!result) return "invalid password";
        
        var token = _jwtProvider.GenerateToken(user);
        
        return token;
    }

    #endregion

    #region GetPaginationUsers

    public PaginationResponse<List<GetUser>> GetPaginationUsers(UserFilter filter)
    {
        var users = _dbContext.Users.AsQueryable();

        if (string.IsNullOrEmpty(filter.UserName))
            users = users.Where(x => x.IsDeleted == false && x.FullName.ToLower()
                .Contains(filter.UserName.ToLower()));
        
        var totalRecords = users.Count();
        
        var res = users.Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize).ToList().Select(x => new GetUser()
            {
                Id = x.Id,
                FullName = x.FullName,
                Role = x.Role,
                ProfileImage = x.ProfileImage,
                CarId = x.CarId
            }).ToList();
        
        return new PaginationResponse<List<GetUser>> (filter.PageNumber, filter.PageSize, totalRecords, res);
    }

    #endregion
    
    
}