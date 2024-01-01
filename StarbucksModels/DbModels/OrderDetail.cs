using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarbucksModels.DbModels
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; } = null!;
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        [ValidateNever]

        public Product Product { get; set; } = null!;

        public decimal Price { get; set; }
    }
}
