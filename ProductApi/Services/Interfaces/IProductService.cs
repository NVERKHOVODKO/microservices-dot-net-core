using TestApplication.DTO;

namespace ProductApi.Services.Interfaces;

public interface IProductService
{
    public Task<Guid> CreateProductAsync(CreateProductRequest request);
    public List<ProductEntity> GetProducts();
    public Task<ProductEntity> GetProduct(Guid id);
    public Task UpdateName(EditNameRequest request);
    public Task UpdateDescription(EditDescriptionRequest request);
    public Task UpdateAvailability(EditAvailabilityRequest request);
    public Task UpdatePrice(EditPriceRequest request);
    public Task DeleteProductAsync(Guid id);
}