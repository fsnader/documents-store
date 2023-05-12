using DocumentsStore.Api.Authorization;
using DocumentsStore.Api.DTOs.Authorization;
using DocumentsStore.Api.DTOs.Users;
using DocumentsStore.UseCases.Users.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsStore.Api.Controllers;

[Route("api/auth")]
[ApiController, AllowAnonymous]
public class AuthController : BaseController
{
    private readonly IUserService _userService;
    private readonly ICreateUser _createUser;

    public AuthController(
        IUserService userService, 
        ICreateUser createUser)
    {
        _userService = userService;
        _createUser = createUser;
    }
    
    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] CreateUserDto user, CancellationToken cancellationToken)
    {
        var result = await _createUser.ExecuteAsync(
            user.ConvertToUser(),
            cancellationToken);

        return UseCaseActionResult(result, UserDto.CreateFromUser);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> GetToken([FromBody] LoginDto login, CancellationToken cancellationToken)
    {
        var token = await _userService.CreateTokenAsync(login.Id, cancellationToken);

        if (token is null)
        {
            return Unauthorized();
        }

        return Ok(token);
    }
}