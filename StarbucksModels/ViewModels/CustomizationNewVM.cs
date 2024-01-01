using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using StarbucksModels.DbModels;

namespace StarbucksModels.ViewModels
{
    public class CustomizationNewVM
    {
        [Key]
        public int CustomizationId { get; set; }
        public string CustomizationName { get; set; }
        public int CustomizationSubcategoryId { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(CustomizationSubcategoryId))]
        public CustomizationSubCategory CustomizationSubCategory { get; set; }

        public string CustomizationCategoryName { get; set; }
        public string CustomizationSubCategoryName { get; set; }


        

    }
}
