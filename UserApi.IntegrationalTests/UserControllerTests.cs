using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Repository;
using TestApplication.Controllers;
using TestApplication.DTO;
using TestApplication.Models;
using TestApplication.Services;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;


namespace UserApiTests;

[TestFixture]
public class UserControllerTests: IClassFixture<WebApplicationFactory<Program>>
{
    [Test]
    public async Task GetUsers_ShouldReturnUsers()
    {
        using (WebApplicationFactory<Program> webApplicationFactory =
               new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { }))
        {
            using (HttpClient httpClient = webApplicationFactory.CreateClient())
            {
                HttpResponseMessage requestMessage = await httpClient.GetAsync("User/users");

                Assert.AreEqual(HttpStatusCode.OK, requestMessage.StatusCode);
            }
        }
    }

    
    
    
    
    
    
    
    
    
    
    
    
    /*[OneTimeSetUp]
    public async Task Setup()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<DataContext>(options =>
                    options.UseInMemoryDatabase("TestDatabase"));

                services.AddScoped<IDbRepository, DbRepository>();
                services.AddScoped<IUserService, UserService>();
                services.AddLogging(builder => builder.AddConsole());
            })
            .Build();

        await _host.StartAsync();

        _userService = _host.Services.GetRequiredService<IUserService>();
    }

    private IHost _host;
    private IUserService _userService;
    private IDbRepository _dbRepository;


    [Test]
    public async Task CreateUserAsync_ValidRequest_UserCreatedSuccessfully()
    {
        var request = new CreateUserRequest
        {
            Login = "TestUser",
            Password = "Password123",
            Email = "testuser@example.com"
        };

        await _userService.CreateUserAsync(request);

        using var scope = _host.Services.CreateScope();
        var dbRepository = scope.ServiceProvider.GetRequiredService<IDbRepository>();
        var user = await dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Login == "TestUser");

        Assert.IsNotNull(user);
        Assert.AreEqual("TestUser", user.Login);
    }*/

    /*[Test]
    public async Task CreateUserAsync_ValidRequest_UserCreatedSuccessfully()
    {
        var createUserRequest = new CreateUserRequest { UserName = "TestUser" };

        await _userService.CreateUserAsync(createUserRequest);

        using var scope = _host.Services.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var user = await userRepository.GetAsync("TestUser");

        Assert.IsNotNull(user);
        Assert.AreEqual("TestUser", user.UserName);
    }*/


    /*[Test]
    public async Task CreateUser_ValidRequest_ReturnsOkResult()
    {
        var controller = new UserController(_userService, null);

        var request = new CreateUserRequest
        {
            Login = "TestUser",
            Password = "Password123",
            Email = "testuser@example.com"
        };

        var result = await controller.CreateUser(request);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public async Task AddRoleToUser_ValidRequest_ReturnsOkResult()
    {
        var controller = new UserController(_userService, null);

        var request = new AddUserRoleRequest
        {
            UserId = Guid.NewGuid(),
            RoleName = "UserRole"
        };

        var result = await controller.AddRoleToUser(request);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public async Task DeleteUser_ValidRequest_ReturnsOkResult()
    {
        var controller = new UserController(_userService, null);

        var request = new DeleteUserRequest
        {
            DeleterId = Guid.NewGuid(),
            DeletedId = Guid.NewGuid()
        };

        var result = await controller.DeleteUser(request.DeleterId, request.DeletedId);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public async Task RemoveUserRole_ValidRequest_ReturnsOkResult()
    {
        var controller = new UserController(_userService, null);

        var request = new RemoveUserRequest
        {
            UserId = Guid.NewGuid(),
            RoleName = "UserRole"
        };

        var result = await controller.RemoveUserRole(request);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public async Task EditLogin_ValidRequest_ReturnsOkResult()
    {
        var controller = new UserController(_userService, null);

        var request = new EditLoginRequest
        {
            UserId = Guid.NewGuid(),
            NewLogin = "NewTestUser"
        };

        var result = await controller.EditLogin(request);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public async Task EditEmail_ValidRequest_ReturnsOkResult()
    {
        var controller = new UserController(_userService, null);

        var request = new EditEmailRequest
        {
            UserId = Guid.NewGuid(),
            NewEmail = "newtestuser@example.com"
        };

        var result = await controller.EditEmail(request);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public async Task EditPassword_ValidRequest_ReturnsOkResult()
    {
        var controller = new UserController(_userService, null);

        var request = new EditPasswordRequest
        {
            Code = "123456",
            NewPassword = "NewPassword123"
        };

        var result = await controller.EditPassword(request);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
    }*/
}