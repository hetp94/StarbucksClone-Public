using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace StarbucksModels.DbModels
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public int MenuId { get; set; }
        [ForeignKey("MenuId")]
        [ValidateNever]
        [DisplayName("Menu Type")]
        public Menu Menu { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("Category Name")]
        public string CategoryName { get; set; }
        [Required]
        public bool ShowVisibility { get; set; } = true;
        public int SortingNumber { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime EfefctiveDate { get; set; } = DateTime.Now;
    }
}
