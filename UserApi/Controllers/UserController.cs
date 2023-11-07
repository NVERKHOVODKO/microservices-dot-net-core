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
        await _userService.CreateUserAsync(request);
        return Ok("User created successfully");
    }


    /*//[Authorize(Roles = "Admin, SuperAdmin, Support")]
    [HttpPost("addRole")]
    public async Task<IActionResult> AddRoleToUser([FromBody] AddUserRoleRequest request)
    {
        try
        {
            await _userService.AddRoleToUserAsync(request);
            return Ok("Role added to user successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Can't add role{request.RoleId} to user{request.UserId}: {ex.Message}");
            return StatusCode(500, $"Can't add role{request.RoleId} to user{request.UserId}: {ex.Message}");
        }
    }*/


    //[Authorize]
    [HttpGet("getUser")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _userService.GetUser(id);
        if (user == null) return NotFound("User not found");
        return Ok(user);
    }


    //[Authorize]
    [HttpGet("getUsers")]
    public async Task<IActionResult> GetUsers(int pageNumber, int pageSize)
    {
        if (pageSize < 1 || pageNumber < 1) return BadRequest("Invalid page number or page size.");

        var user = _userService.GetUsers();
        if (user == null) return NotFound("User not found");
        return Ok(user);
    }


    /*[Authorize(Roles = "Admin, SuperAdmin, Support")]
    [HttpPost("filterSortUsers")]
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


    /*//[Authorize(Roles = "Admin, SuperAdmin")]
    [HttpPatch("editLogin")]
    public async Task<IActionResult> EditUser(EditLoginRequest request)
    {
        try
        {
            if (request.NewLogin == null || request.UserId == null) return BadRequest("Fill in all details");
            if (request.NewLogin.Length > 20) return BadRequest("Login has to be shorter that 20 symbols");
            if (request.NewLogin.Length < 4) return BadRequest("Login has to be longer that 4 symbols");
            if (!await _userService.IsLoginUniqueForUserAsync(request.UserId, request.NewLogin))
                return BadRequest("Login isn't unique");
            await _userService.EditLoginAsync(request);
            return Ok($"User with Id {request.UserId} has been updated.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error editing user with Id {request.UserId}: {ex.Message}");
            return StatusCode(500, $"User with Id {request.UserId} hasn't been updated.");
        }
    }


    //[Authorize(Roles = "SuperAdmin")]
    [HttpDelete("deleteUser")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        if (id == null) return BadRequest("Id must be not null");
        try
        {
            await _userService.DeleteUserAsync(id);
            _logger.LogInformation($"User({id}) has been deleted.");
            return Ok($"User({id}) has been deleted.");
        }
        catch (DbUpdateException e)
        {
            _logger.LogError($"User({id}) hasn't been deleted.");
            return StatusCode(StatusCodes.Status500InternalServerError, $"User({id}) hasn't been deleted.");
        }
    }*/
}