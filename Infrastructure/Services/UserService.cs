using System.Net;
using System.Xml;
using Domain.DTOs.User;
using Domain.Entities;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Logic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    #region Constructor

    private readonly IPasswordHasher _passwordHasher;
    private readonly ApplicationDbContext _dbContext;
    private readonly IFileStorageService _fileService;
    private readonly IJwtProvider _jwtProvider;
    private readonly ISandMail _mail;
    private readonly ILogic _logic;
    private readonly ILogger<UserService> _logger;

    public UserService(IPasswordHasher passwordHasher,
        ApplicationDbContext dbContext,
        IFileStorageService fileService,
        IJwtProvider jwtProvider,
        ISandMail mail, ILogic logic,
        ILogger<UserService> logger)
    {
        _passwordHasher = passwordHasher;
        _dbContext = dbContext;
        _fileService = fileService;
        _jwtProvider = jwtProvider;
        _mail = mail;
        _logic = logic;
        _logger = logger;
    }

    #endregion

    #region Register

    public async Task<Response<string>> Register(CreatedUser dto)
    {
        var res = _logic.ValidatorPassword(dto.Password);
        if (!res)
            return new Response<string>(HttpStatusCode.BadRequest, "invalid password");
        
        var hashedPassword = _passwordHasher.Generate(dto.Password);

        var created = new User()
        {
            FullName = dto.FullName,
            PasswordHash = hashedPassword,
            Email = dto.Email,
            Role = dto.Role,
        };

        if (dto.ProfileImage != null)
            created.ProfileImage = await _fileService.SaveFileAsync(dto.ProfileImage, "images");

        _dbContext.Users.Add(created);

        await _dbContext.SaveChangesAsync();
        
        await _mail.SendAsync(created, dto.Password);

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

        if (!result) return "invalid password";

        var token = _jwtProvider.GenerateToken(user);

        return token;
    }

    #endregion

    #region GetPaginationUsers

    public PaginationResponse<List<GetUser>> GetPaginationUsers(UserFilter filter)
    {
        var users = _dbContext.Users.AsQueryable();
        _logger.LogInformation("Get Users in DataBase");

        if (!string.IsNullOrEmpty(filter.UserName))
            users = users.Where(x => x.FullName.ToLower()
                .Contains(filter.UserName.ToLower()));
        _logger.LogInformation($"Search User By FullName {filter.UserName}");

        var totalRecords = users.Count();
        _logger.LogInformation($"Get totalRecords: {totalRecords} in user.Count");

        var res = users.Where(x => !x.IsDeleted)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize).ToList()
            .Select(x => new GetUser()
            {
                Id = x.Id,
                FullName = x.FullName,
                Role = x.Role,
                ProfileImage = x.ProfileImage,
                CreatedAt = x.CreatedAt
            }).ToList();

        return new PaginationResponse<List<GetUser>>(filter.PageNumber, filter.PageSize, totalRecords, res);
    }

    #endregion

    #region GetUserById

    public Response<GetUser> GetUserById(int userId)
    {
        var res = _dbContext.Users.FirstOrDefault(x => x.Id == userId && !x.IsDeleted);
        
        if(res == null)
            return new Response<GetUser>(HttpStatusCode.NotFound, "User not found");

        var getUser = new GetUser()
        {
            Id = res.Id,
            FullName = res.FullName,
            Role = res.Role,
            ProfileImage = res.ProfileImage,
            CreatedAt = res.CreatedAt
        };
        
        return new Response<GetUser>(getUser);
    }

    #endregion

    #region UpdateUser

    public async Task<Response<string>> UpdateUser(int id, UpdateUser dto)
    {
        var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);
        if (user == null)
            return new Response<string>(HttpStatusCode.NotFound, "not found");

        user.FullName = dto.FullName ?? user.FullName;
        if (dto.ProfileImage != null)
            user.ProfileImage = await _fileService.SaveFileAsync(dto.ProfileImage, "images");

        await _dbContext.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK, "Updated user");
    }

    #endregion

    #region UpdatePasswordUser
    public async Task<Response<string>> UpdatePasswordUser(int id, UpdatePasswordUser dto)
    {
        var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);
        if (user == null)
            return new Response<string>(HttpStatusCode.NotFound, "not found");

        if (string.IsNullOrEmpty(dto.LastPassword) || string.IsNullOrEmpty(dto.NewPassword))
            return new Response<string>(HttpStatusCode.BadRequest, "invalid password");
        
        if (!_passwordHasher.Verify(dto.LastPassword, user.PasswordHash))
            return new Response<string>(HttpStatusCode.Unauthorized, "Current password is incorrect");
        
        if (!_logic.ValidatorPassword(dto.NewPassword))
            return new Response<string> (HttpStatusCode.BadRequest, "invalid password");
        
        user.PasswordHash = _passwordHasher.Generate(dto.NewPassword);
        await _dbContext.SaveChangesAsync();
        
        return new Response<string>(HttpStatusCode.OK, "Updated password in user");
    }

    #endregion

    #region DeleteUser

    public async Task<Response<string>> DeleteUser(int id)
    {
        var user = _dbContext.Users.FirstOrDefault(x => !x.IsDeleted && x.Id == id);
        if (user == null)
            return new Response<string>(HttpStatusCode.NotFound, "not found");
        user.IsDeleted = true;

        await _dbContext.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK, "Deleted user");
    }

    #endregion
}