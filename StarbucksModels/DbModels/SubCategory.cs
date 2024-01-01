using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace StarbucksModels.DbModels
{
    public class SubCategory
    {
        [Key]
        public int SubcategoryId { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        [DisplayName("Category Name")]
        public Category Category { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("Sub Category Name")]
        public string SubCategoryName { get; set; }
        [NotMapped]
        [DisplayName("Menu Type")]
        public int MenuId { get; set; }
        [Required]
        [DisplayName("Visibility")]
        public bool ShowItem { get; set; } = false;
        public int SortingNumber { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime EffectiveDate { get; set; } = DateTime.Now;
    }
}
