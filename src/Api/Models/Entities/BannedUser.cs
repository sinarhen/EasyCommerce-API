﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("UserId")]
public class BannedUser
{
    public string UserId { get; set; }
    public string Reason { get; set; }
    public DateTime BanStartTime { get; set; }
    public DateTime BanEndTime { get; set; }
    
    // Navigation properties
    [ForeignKey("UserId")]
    public User User { get; set; }
}