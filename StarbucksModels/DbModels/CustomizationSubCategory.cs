using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarbucksModels.DbModels
{
    public class CustomizationSubCategory
    {
        [Key]
        public int CustomizationSubcategoryId { get; set; }
        public string SubCategoryName { get; set; }

        
        public int CustomizationCategoryId { get; set; }

        [ForeignKey(nameof(CustomizationCategoryId))]
        [ValidateNever]
        public CustomizationCategory CustomizationCategory { get; set; }
    }
}
