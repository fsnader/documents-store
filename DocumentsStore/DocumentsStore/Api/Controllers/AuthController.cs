using DocumentsStore.Api.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsStore.Api.Controllers;

[Route("api/auth")]
[ApiController, AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> GetToken([FromForm] int userId, CancellationToken cancellationToken)
    {
        var token = await _userService.CreateTokenAsync(userId, cancellationToken);

        if (token is null)
        {
            return Unauthorized();
        }

        return Ok(token);
    }
}