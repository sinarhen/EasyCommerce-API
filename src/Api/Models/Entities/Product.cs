﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("Id")]
public class Product
{
    [Key]
    public Guid Id { get; set; }
    
    [Key]
    public Guid CategoryId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }


    public double? Discount { get; set; }

    [Key]
    public Guid? OccasionId { get; set; }


    public string SizeChartImageUrl { get; set; }
    public Gender? Gender { get; set; }

    public Season? Season { get; set; }

    public Guid MainMaterialId { get; set; }
    public int? CollectionYear { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
    [ForeignKey("OccasionId")]
    public Occasion Occasion { get; set; }

    [ForeignKey("MainMaterialId")]
    public Material MainMaterial { get; set; }
    
    public ICollection<ProductMaterial> Materials { get; set; } = new List<ProductMaterial>();
    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public ICollection<ProductStock> Stocks { get; set; } = new List<ProductStock>();
}

public enum Gender
{
    Male,
    Female,
    Unisex
}

public enum Season
{
    Winter,
    Spring,
    Summer,
    Autumn,
    All,
    DemiSeason,
}