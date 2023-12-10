using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProductApi.Services;
using ProductApi.Services.Interfaces;
using ProjectX.Exceptions;
using Repository;
using TestApplication.DTO;

namespace ProductAPI.UnitTests;

public class ProductServiceTests
{
    [Fact]
    public async Task CreateProductAsync_ValidRequest_ReturnsProductId()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase("test")
            .Options;

        var dbContext = new DataContext(options);
        var dbRepository = new DbRepository(dbContext);

        var loggerMock = new Mock<ILogger<ProductService>>();
        var productService = new ProductService(dbRepository, loggerMock.Object);

        var request = new CreateProductRequest
        {
            Name = "TestProduct",
            Price = 10.0m,
            Availability = true,
            CreatorId = Guid.NewGuid(),
            Description = "Test Description"
        };

        // Act
        var result = await productService.CreateProductAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
    }
}