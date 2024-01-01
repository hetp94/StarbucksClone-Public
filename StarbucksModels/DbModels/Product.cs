using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace StarbucksModels.DbModels
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
        [Required]
        [MaxLength(1024)]
        public string Descrption { get; set; }
        [Required]
        public int? RewardPoints { get; set; }
      
        [MaxLength(100)]
        public string? NutritionInfoText { get; set; }

        public int MenuId { get; set; }
        [ForeignKey("MenuId")]
        [ValidateNever]
        public Menu Menu { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        public int SubCategoryId { get; set; }
        [ForeignKey("SubCategoryId")]
        [ValidateNever]
        public SubCategory SubCategory { get; set; }
        [MaxLength(255)]
        [DisplayName("Full Product Image")]
        public string? ImageUrl { get; set; }

        [MaxLength(255)]
        [DisplayName("Cropped Product Image")]
        public string? CroppedUrl { get; set; }

        public int? Calories { get; set; }

        [DisplayName("Price")]
        public decimal ItemPrice { get; set; }
        [NotMapped]
        [ValidateNever]
        public ProductCustomization ProductCustomization { get; set; }

        [NotMapped]
        [ValidateNever]
        public CustomizationVisibility CustomizationVisibility { get; set; }

        [NotMapped]
        [ValidateNever]
        public IFormFile? WholeImage { get; set; }
        [NotMapped]
        [ValidateNever]
        public IFormFile? CroppedImage { get; set; }
        public int? SortingOrder { get; set; } 
    }
}
