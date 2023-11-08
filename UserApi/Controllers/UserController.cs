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

    //[Authorize(Roles = "Admin, SuperAdmin")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var id = await _userService.CreateUserAsync(request);
        return Ok(id);
    }


    //[Authorize(Roles = "Admin, SuperAdmin, Support")]
    [HttpPost("addRole")]
    public async Task<IActionResult> AddRoleToUser([FromBody] AddUserRoleRequest request)
    {
        await _userService.AddRoleToUserAsync(request);
        return Ok("Role added to user successfully");
    }


    //[Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _userService.GetUser(id);
        if (user == null) return NotFound("User not found");
        return Ok(user);
    }


    //[Authorize]
    [HttpGet("")]
    public async Task<IActionResult> GetUsers(int pageNumber, int pageSize)
    {
        if (pageSize < 1 || pageNumber < 1) return BadRequest("Invalid page number or page size.");

        var user = _userService.GetUsers();
        if (user == null) return NotFound("User not found");
        return Ok(user);
    }
    
    
    //[Authorize(Roles = "SuperAdmin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        await _userService.DeleteUserAsync(id);
        return Ok($"User({id}) has been deleted.");
    }

    //[Authorize(Roles = "SuperAdmin")]
    [HttpDelete("removeUserRole")]
    public async Task<IActionResult> RemoveUserRole(Guid id)
    {
        await _userService.RemoveUserRoleAsync(id);
        return Ok($"User({id}) has been deleted.");
    }

    //[Authorize(Roles = "Admin, SuperAdmin")]
    [HttpPatch("editLogin")]
    public async Task<IActionResult> EditLogin(EditLoginRequest request)
    {
        await _userService.UpdateLogin(request);
        return Ok($"User with Id {request.UserId} has been updated.");
    }

    //[Authorize(Roles = "Admin, SuperAdmin")]
    [HttpPatch("editEmail")]
    public async Task<IActionResult> EditEmail(EditEmailRequest request)
    {
        await _userService.UpdateEmail(request);
        return Ok($"User with Id {request.UserId} has been updated.");
    }
}