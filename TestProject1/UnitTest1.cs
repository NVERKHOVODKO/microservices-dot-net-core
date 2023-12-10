using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace TestProject1;

public class UnitTest1: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory;

    public UnitTest1(WebApplicationFactory<Program> webApplicationFactory)
    {
        _webApplicationFactory = webApplicationFactory;
    }
    
    [Fact]
    public async Task GetUsers_ShouldReturnUsers()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync("/User/users");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        
        Assert.DoesNotContain("User not found", content);
    }
    
    
    [Fact]
    public async Task GetUsers_1ShouldReturnUsers()
    {
        
    }
}