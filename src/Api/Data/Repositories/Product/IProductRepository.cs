using ECommerce.Models.DTOs.Product;
using ECommerce.RequestHelpers.SearchParams;

namespace ECommerce.Data.Repositories.Product;

public interface IProductRepository
{
    Task<(IEnumerable<ProductDto>, ProductFiltersDto)> GetProductsAsync(ProductSearchParams searchParams, string userId);
    Task<ProductDetailsDto> GetProductAsync(Guid id, string userId);    
    Task<Models.Entities.Product> CreateProductAsync(CreateProductDto productDto, string userId, bool isAdmin);
    Task<Models.Entities.Product> UpdateProductAsync(Guid id, UpdateProductDto productDto, string userId, bool isAdmin);
    Task DeleteProductAsync(Guid id, string userId, bool isAdmin);
}