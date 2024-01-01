using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarbucksModels.DbModels
{
    public class ProdCustVisibility
    {
        [Key]
        public int ProdCustVisibilityId { get; set; }
        public int ProductId { get; set; }
        public int CustomizationVisibilityCode { get; set; }

        [ForeignKey(nameof(CustomizationVisibilityCode))]
        [ValidateNever]
        public CustomizationCategory CustomizationVisibilityCodeNavigation { get; set; } = null!;
        [ForeignKey(nameof(ProductId))]
        [ValidateNever]
        public Product Product { get; set; } = null!;
    }
}
