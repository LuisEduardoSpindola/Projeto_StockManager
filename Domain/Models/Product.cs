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
    [Display(Name = "Nome do produto")]
    public string ProductName { get; set; }

    [StringLength(500)]
    [Display(Name = "Descrição")]
    public string Description { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    [Display(Name = "Valor de compra")]
    public decimal BuyValue { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    [Display(Name = "Valor de venda")]
    public decimal? SellValue { get; set; }

    [Column("MinimunQTY")]
    [Display(Name = "Quantidade mínima")]
    public int? MinimunQty { get; set; }

    [Display(Name = "Imagem do produto")]
    public string Img { get; set; }

    [InverseProperty("StockProduct")]
    public virtual ICollection<StockItem> StockItems { get; set; } = new List<StockItem>();
}