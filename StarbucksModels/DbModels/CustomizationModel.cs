using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarbucksModels.DbModels
{
    public class CustomizationModel
    {
        [Key]
        public int Id { get; set; }
        public string CustomizationType { get; set; }
        public string? SubCustomizationType { get; set; }
        public string? Customization { get; set; }
        public string? SubCustomization { get; set; }
       


        [NotMapped]
        [ValidateNever]
        public IEnumerable<SelectListItem> CustomizationTypeDropDown { get; set; }

        [NotMapped]
        [ValidateNever]
        public IEnumerable<SelectListItem> SubCustomizationTypeDropDown { get; set; }

        [NotMapped]
        [ValidateNever]
        public IEnumerable<SelectListItem> CustomizationDropDown { get; set; }
    }
}
