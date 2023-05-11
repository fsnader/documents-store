using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using DocumentsStore.Domain;
using DocumentsStore.UseCases.Users.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace DocumentsStore.Api.Authorization;

public class UserService : IUserService
{
    private readonly IGetUserById _getUserById;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public UserService(
        IGetUserById getUserById,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
    {
        _getUserById = getUserById;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }
    
    public async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var id = GetUserId();

        if (id is null)
        {
            throw new InvalidCredentialException();
        }
        
        var result = await _getUserById.ExecuteAsync(id.Value, cancellationToken);
        return result.Result!;
    }

    public async Task<string?> CreateTokenAsync(int userId, CancellationToken cancellationToken)
    {
        var result = await _getUserById.ExecuteAsync(userId, cancellationToken);

        if (result.Result is null)
        {
            return null;
        }
        
        var user = result.Result;
        
        List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    private int? GetUserId()
    {
        var result = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (result is null)
        {
            return null;
        }
        
        return int.Parse(result);
    }
}