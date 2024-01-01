using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarbucksModels.DbModels
{
    public class Customization
    {
        [Key]
        public int CustomizationId { get; set; }
        public string CustomizationName { get; set; }
        public int CustomizationSubcategoryId { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(CustomizationSubcategoryId))]
        public CustomizationSubCategory CustomizationSubCategory { get; set; }

    }
}
