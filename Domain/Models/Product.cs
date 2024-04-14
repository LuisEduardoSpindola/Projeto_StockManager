﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public partial class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    [StringLength(100)]
    public string ProductName { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal BuyValue { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? SellValue { get; set; }

    [Column("MinimunQTY")]
    public int? MinimunQty { get; set; }

    public string Img { get; set; }

    [InverseProperty("StockProduct")]
    public virtual ICollection<StockItem> StockItems { get; set; } = new List<StockItem>();
}