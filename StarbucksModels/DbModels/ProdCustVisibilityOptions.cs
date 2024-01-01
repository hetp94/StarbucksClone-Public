using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StarbucksModels.DbModels
{
    public class ProdCustVisibilityOptions
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        [ValidateNever]
        public Product Product { get; set; } = null!;


        public int CustomizationOptionId { get; set; }
        [ForeignKey(nameof(CustomizationOptionId))]
        [ValidateNever]
        public CustomizationSubCategory CustomizationVisibilityOptions { get; set; } = null!;
      
    }
}
