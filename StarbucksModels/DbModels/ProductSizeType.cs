using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarbucksModels.DbModels
{
    public class ProductSizeType
    {
        [Key]
        public int ProductSizeTypeId { get; set; }
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        [ValidateNever]
        public Product Product { get; set; }



        public int SizeTypeId { get; set; }
        [ForeignKey(nameof(SizeTypeId))]
        [ValidateNever]
        public SizeType SizeType { get; set; }
    }
}
