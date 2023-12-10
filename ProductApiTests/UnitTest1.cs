using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProductApiTests
{
    public class UnitTest1 : IAsyncLifetime
    {
        private IHost _host;
        private IUserService _userService;

        public async Task InitializeAsync()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<DataContext>(options =>
                        options.UseInMemoryDatabase("TestDatabase"));

                    services.AddScoped<IDb, >();
                    services.AddScoped<IUserService, UserService>();
                    services.AddLogging(builder => builder.AddConsole());
                })
                .Build();

            await _host.StartAsync();

            _userService = _host.Services.GetRequiredService<IUserService>();
        }

        [Fact]
        public async Task CreateUserAsync_ValidRequest_UserCreatedSuccessfully()
        {
            var createUserRequest = new CreateUserRequest { UserName = "TestUser" };

            await _userService.CreateUserAsync(createUserRequest);

            using var scope = _host.Services.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var user = await userRepository.GetAsync("TestUser");

            Assert.NotNull(user);
            Assert.Equal("TestUser", user.UserName);
        }

        public async Task DisposeAsync()
        {
            await _host.StopAsync();
            _host.Dispose();
        }
    }
}