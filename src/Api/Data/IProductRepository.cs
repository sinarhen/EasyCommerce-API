using Ecommerce.Models.DTOs;
using ECommerce.Models.DTOs;
using ECommerce.Models.Entities;
using Ecommerce.RequestHelpers;

namespace Ecommerce.Data;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync(SearchParams searchParams);
    public Task<Product> GetProductAsync(Guid id);
    Task<Product> CreateProductAsync(CreateProductDto product, string username, IEnumerable<string> role);
    Task<Product> UpdateProductAsync(Guid id, UpdateProductDto product, string username, IEnumerable<string> role);
    Task DeleteProductAsync(Guid id);
    
}