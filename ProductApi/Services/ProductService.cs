using Microsoft.EntityFrameworkCore;
using ProductApi.Services.Interfaces;
using ProjectX.Exceptions;
using Repository;
using TestApplication.DTO;
using AccessViolationException = ProjectX.Exceptions.AccessViolationException;

namespace ProductApi.Services;

public class ProductService : IProductService
{
    private readonly IDbRepository _dbRepository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IDbRepository dbRepository, ILogger<ProductService> logger)
    {
        _logger = logger;
        _dbRepository = dbRepository;
    }

    public async Task<Guid> CreateProductAsync(CreateProductRequest request)
    {
        if (request.Name.Length < 4) throw new IncorrectDataException("Name can't be less than 4 symbols");
        if (request.Price == null) throw new IncorrectDataException("Price can't be null");
        if (request.Availability == null) throw new IncorrectDataException("Availability can't be null");
        if (request.CreatorId == null) throw new IncorrectDataException("CreatorId can't be null");
        if (request.Price < 0) throw new IncorrectDataException("Price can't be less that zero");
        if (request.Description.Length > 200)
            throw new IncorrectDataException("Description length can't be more that 200 symbols");

        var id = Guid.NewGuid();
        var entity = new ProductEntity
        {
            Id = id,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Availability = request.Availability,
            CreatorId = request.CreatorId,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow
        };
        var result = await _dbRepository.Add(entity);
        await _dbRepository.SaveChangesAsync();
        return id;
    }

    public List<ProductEntity> GetProducts()
    {
        var products = _dbRepository.GetAll<ProductEntity>().ToList();
        if (products == null) throw new EntityNotFoundException("There is no products");
        return products;
    }


    public async Task<ProductEntity> GetProduct(Guid id)
    {
        _logger.LogInformation($"GetProduct(Guid id): {id}");
        var product = await _dbRepository.Get<ProductEntity>().FirstOrDefaultAsync(x => x.Id == id);
        if (product == null) throw new EntityNotFoundException("Product not found");
        return product;
    }


    public async Task UpdateName(EditNameRequest request)
    {
        var product = await _dbRepository.Get<ProductEntity>().FirstOrDefaultAsync(x => x.Id == request.ProductId);
        if (product.CreatorId != request.UserId) throw new AccessViolationException("You can't edit this product");
        if (product == null) throw new EntityNotFoundException("Product not found");
        if (request.NewName == null) throw new IncorrectDataException("Fill in all details");
        if (request.NewName.Length < 4) throw new IncorrectDataException("Name can't be than 4 symbols");
        if (request.NewName.Length > 20) throw new IncorrectDataException("Name has to be shorter that 20 symbols");
        product.Name = request.NewName;
        product.DateUpdated = DateTime.UtcNow;
        await _dbRepository.Update(product);
        await _dbRepository.SaveChangesAsync();
    }


    public async Task UpdateDescription(EditDescriptionRequest request)
    {
        var product = await _dbRepository.Get<ProductEntity>().FirstOrDefaultAsync(x => x.Id == request.ProductId);
        if (product.CreatorId != request.UserId) throw new AccessViolationException("You can't edit this product");
        if (product == null) throw new EntityNotFoundException("Product not found");
        if (request.NewDescription == null) throw new IncorrectDataException("Fill in all details");
        if (request.NewDescription.Length > 200)
            throw new IncorrectDataException("Description has to be shorter that 200 symbols");
        product.Name = request.NewDescription;
        product.DateUpdated = DateTime.UtcNow;
        await _dbRepository.Update(product);
        await _dbRepository.SaveChangesAsync();
    }

    public async Task UpdatePrice(EditPriceRequest request)
    {
        var product = await _dbRepository.Get<ProductEntity>().FirstOrDefaultAsync(x => x.Id == request.ProductId);
        if (product.CreatorId != request.UserId) throw new AccessViolationException("You can't edit this product");
        if (product == null) throw new EntityNotFoundException("Product not found");
        if (request.NewPrice < 0) throw new IncorrectDataException("Price must be positive value");
        product.Price = request.NewPrice;
        product.DateUpdated = DateTime.UtcNow;
        await _dbRepository.Update(product);
        await _dbRepository.SaveChangesAsync();
    }

    public async Task UpdateAvailability(EditAvailabilityRequest request)
    {
        var product = await _dbRepository.Get<ProductEntity>().FirstOrDefaultAsync(x => x.Id == request.ProductId);
        if (product.CreatorId != request.UserId) throw new AccessViolationException("You can't edit this product");
        if (product == null) throw new EntityNotFoundException("Product not found");
        if (request.NewAvailability == null || request.UserId == null)
            throw new IncorrectDataException("Fill in all details");
        product.Availability = request.NewAvailability;
        product.DateUpdated = DateTime.UtcNow;
        await _dbRepository.Update(product);
        await _dbRepository.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(Guid id)
    {
        var user = await _dbRepository.Get<ProductEntity>().FirstOrDefaultAsync(x => x.Id == id);
        if (user == null) throw new EntityNotFoundException("Product not found");
        await _dbRepository.Delete<ProductEntity>(id);
        await _dbRepository.SaveChangesAsync();
    }
}