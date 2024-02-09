﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("ProductId", "MaterialId")]
public class ProductMaterial : BaseEntity
{
    [Key] public Guid ProductId { get; set; }

    [Key] public Guid MaterialId { get; set; }

    public double Percentage { get; set; }

    // Navigation properties
    [ForeignKey("MaterialId")] public Material Material { get; set; }

    [ForeignKey("ProductId")] public Product Product { get; set; }
}