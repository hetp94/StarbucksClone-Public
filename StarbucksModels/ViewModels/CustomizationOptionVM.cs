using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using StarbucksModels.DbModels;

namespace StarbucksModels.ViewModels
{
    public class CustomizationOptionVM
    {
        [Key]
        public int CustomizationOptionId { get; set; }
        public int CustomizationId { get; set; }
        public string CustomizationOptionName { get; set; }

        [ForeignKey(nameof(CustomizationId))]
        [ValidateNever]
        public Customization CustomizationNew { get; set; }

        public string CustomizationCategoryName { get; set; }
        public string CustomizationSubCategoryName { get; set; }
        public string CustomizationName { get; set; }
    }
}
