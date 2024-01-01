using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using StarbucksModels.DbModels;

namespace StarbucksModels.ViewModels
{
    public class ProductCustomizationVM
    {
        [Key]
        public int ProductCustomizationKey { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }

      
        public int? CustomizationCategoryId { get; set; }
        public string? CustomizationCategoryName { get; set; }

        public int? CustomizationSubcategoryId { get; set; }
        public string? CustomizationSubcategoryName { get; set; }

        public int? CustomizationId { get; set; }
        public string? CustomizationName { get; set; }

        public int? CustomizationOptionId { get; set; }
        public string? CustomizationOptionName { get; set; }

        public int? CustomizationOptionId2 { get; set; }
        public string? CustomizationOptionName2 { get; set; }

        public int? Qty { get; set; }
        public int SortingOrder { get; set; } = 1;


        public List<CustomizationOption> AllCustomizationOptions { get; set; } // List to store all CustomizationOptions
    }
}
