﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("Id")]
public class Order : BaseEntity
{
    [Key] public Guid Id { get; set; }

    [Key] public string CustomerId { get; set; }
    
    public OrderStatus Status { get; set; }
    
    // Navigation properties
    [ForeignKey("CustomerId")] public User Customer { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

public enum OrderStatus
{
    Pending,
    Accepted
}