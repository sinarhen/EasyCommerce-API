﻿using ECommerce.Models.DTOs.Product;
using ECommerce.RequestHelpers;

namespace ECommerce.Data.Repositories.Product;

public interface IProductRepository
{
    Task<IEnumerable<ProductDto>> GetProductsAsync(ProductSearchParams searchParams);
    Task<ProductDto> GetProductAsync(Guid id);
    Task<Models.Entities.Product> CreateProductAsync(CreateProductDto productDto, string userId, bool isAdmin);
    Task<Models.Entities.Product> UpdateProductAsync(Guid id, UpdateProductDto productDto, string userId, bool isAdmin);
    Task DeleteProductAsync(Guid id, string userId, bool isAdmin);
}