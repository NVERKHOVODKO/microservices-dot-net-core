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
        //if (!await _userService.IsLoginUniqueAsync(request.Login)) return BadRequest("Login isn't unique");
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
    [HttpPatch("")]
    public async Task<IActionResult> EditLogin(EditUserRequest request)
    {
        /*if (request.NewLogin == null || request.UserId == null) return BadRequest("Fill in all details");
        if (request.NewLogin.Length > 20) return BadRequest("Login has to be shorter that 20 symbols");
        if (request.NewLogin.Length < 4) return BadRequest("Login has to be longer that 4 symbols");
        if (!await _userService.IsLoginUniqueForUserAsync(request.UserId, request.NewLogin))
            return BadRequest("Login isn't unique");*/

        await _userService.Update(request);
        return Ok($"User with Id {request.UserId} has been updated.");
    }

    //[Authorize(Roles = "Admin, SuperAdmin, Support")]
    /*[HttpPost("filterSortUsers")]
    public async Task<IActionResult> FilterSortUsers(FilterSortUserRequest request)
    {
        if (request.PageNumber < 1 || request.PageSize < 1)
        {
            return BadRequest("Invalid page number or page size.");
        }
        try
        {
            var filteredUsers = await _userService.GetFilteredAndSortedUsers(request);
            return Ok(filteredUsers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error filtering/sorting users: {ex.Message}");
            return StatusCode(500, $"Error filtering/sorting users: {ex.Message}");
        }
    }*/


    /*[Authorize(Roles = "Admin, SuperAdmin, Support")]
    [HttpPost("filterSortRoles")]
    public async Task<IActionResult> FilterSortUsersRoles(FilterSortRolesRequest request)
    {
        if (request.PageNumber < 1 || request.PageSize < 1)
        {
            return BadRequest("Invalid page number or page size.");
        }
        try
        {
            var filteredRoles = await _userService.GetFilteredAndSortedRoles(request);
            return Ok(filteredRoles);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error filtering/sorting roles: {ex.Message}");
            return StatusCode(500, $"Error filtering/sorting roles: {ex.Message}");
        }
    }*/
}