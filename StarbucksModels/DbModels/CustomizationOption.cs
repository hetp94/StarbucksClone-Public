using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarbucksModels.DbModels
{
    public class CustomizationOption
    {
        [Key]
        public int CustomizationOptionId { get; set; }
        public int CustomizationId { get; set; }
        public string CustomizationOptionName { get; set; }

        [ForeignKey(nameof(CustomizationId))]
        [ValidateNever]
        public Customization CustomizationNew { get; set; }

       

    }
}
