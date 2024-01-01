using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace StarbucksModels.DbModels
{
    public class ProductCustomization
    {
        [Key]
        public int ProductCustomizationKey { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
       
        [DisplayName("Customization Type")]
        public int? CustomizationCategoryId { get; set; }
      
        [DisplayName("Sub Customization Type")]
        public int? CustomizationSubcategoryId { get; set; }
       
        [DisplayName("Customization")]
        public int? CustomizationId { get; set; }
       
        [DisplayName("Sub Customization")]
        public int? CustomizationOptionId { get; set; }
        [DisplayName("Quantity")]
        public int? Qty { get; set; }
        public int SortingOrder { get; set; } = 1;
    }
}
