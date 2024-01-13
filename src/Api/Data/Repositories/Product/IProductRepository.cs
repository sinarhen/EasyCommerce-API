using ECommerce.Models.DTOs;
using ECommerce.Models.DTOs.Product;
using ECommerce.RequestHelpers;

namespace ECommerce.Data.Repositories.Product;

public interface IProductRepository
{
    Task<IEnumerable<Models.Entities.Product>>GetProductsAsync(ProductSearchParams searchParams);
    Task<Models.Entities.Product> GetProductAsync(Guid id);
    Task<Models.Entities.Product> CreateProductAsync(CreateProductDto product, string username, IEnumerable<string> role);
    Task<Models.Entities.Product> UpdateProductAsync(Guid id, UpdateProductDto product, string username, IEnumerable<string> role);
    Task DeleteProductAsync(Guid id);
    
}