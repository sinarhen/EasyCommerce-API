﻿using ECommerce.Models.DTOs.Color;
using ECommerce.Models.Entities;

namespace ECommerce.Models.DTOs.Product;

public class ProductDto : BaseEntity
{
    public Guid Id { get; set; }
    public List<ProductCategoryDto> Categories { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public double? Discount { get; set; }
    public IdNameDto Occasion { get; set; }
    public IdNameDto Collection { get; set; }
    // public IdNameDto MainMaterial { get; set; }
    public string Gender { get; set; }
    public string Season { get; set; }
    
    public bool IsFavorite { get; set; }
    // public int OrdersCount { get; set; }
    // public int ReviewsCount { get; set; }
    public double AvgRating { get; set; }
    public decimal MinPrice { get; set; }
    public bool IsNew { get; set; }
    // public bool IsOnSale { get; set; }

    public bool IsAvailable { get; set; }
    public bool IsBestseller { get; set; }
    public List<ColorDto> Colors { get; set; }

    public List<ProductImageDto> Images { get; set; }
}


public class ProductCategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public int Order { get; set; }
}