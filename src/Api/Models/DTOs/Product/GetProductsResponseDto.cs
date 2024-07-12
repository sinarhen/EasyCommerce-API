namespace ECommerce.Models.DTOs.Product;

public record GetProductsResponseDto(ProductDto[] products, int total, int pageNumber, int pageSize);