﻿namespace ECommerce.Models.DTOs;

public class CartItemDto
{
    public CartItemProductDto Product { get; set; }    
    public int Quantity { get; set; }
    public string CustomerId { get; set; }
}