using Microsoft.AspNetCore.Mvc;
using TestApplication.DTO;
using TestApplication.Services;

namespace TestApplication.Controllers;

//[Authorize]
[ApiController]
[Produces("application/json")]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    //[Authorize]
    [HttpPost("users")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var id = await _userService.CreateUserAsync(request);
        return Ok(id);
    }


    //[Authorize]
    [HttpPost("users/add-role")]
    public async Task<IActionResult> AddRoleToUser([FromBody] AddUserRoleRequest request)
    {
        await _userService.AddRoleToUserAsync(request);
        return Ok("Role added to user successfully");
    }


    //[Authorize]
    [HttpGet("users/{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _userService.GetUser(id);
        if (user == null) return NotFound("User not found");
        return Ok(user);
    }


    //[Authorize]
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var user = _userService.GetUsers();
        if (user == null) return NotFound("User not found");
        return Ok(user);
    }


    //[Authorize]
    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(Guid deleterId, Guid deletedId)
    {
        await _userService.DeleteUserAsync(new DeleteUserRequest
        {
            DeleterId = deleterId,
            DeletedId = deletedId,
        });
        return Ok($"User({deletedId}) has been deleted.");
    }

    //[Authorize(Roles = "SuperAdmin, Admin")]
    [HttpDelete("users/remove-user-role")]
    public async Task<IActionResult> RemoveUserRole(RemoveUserRequest request)
    {
        await _userService.RemoveUserRoleAsync(request);
        return Ok($"User({request.UserId}) has been deleted.");
    }

    //[Authorize]
    [HttpPatch("users/edit-login")]
    public async Task<IActionResult> EditLogin(EditLoginRequest request)
    {
        await _userService.UpdateLogin(request);
        return Ok($"User with Id {request.UserId} has been updated.");
    }

    //[Authorize]
    [HttpPatch("users/edit-email")]
    public async Task<IActionResult> EditEmail(EditEmailRequest request)
    {
        await _userService.UpdateEmail(request);
        return Ok($"User with Id {request.UserId} has been updated.");
    }

    [HttpPatch("users/edit-password")]
    public async Task<IActionResult> EditPassword(EditPasswordRequest request)
    {
        await _userService.UpdatePassword(request);
        return Ok();
    }
}