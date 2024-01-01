using System.ComponentModel.DataAnnotations;

namespace StarbucksModels.DbModels
{
    public class CustomizationCategory
    {
        [Key]
        public int CustomizationCategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
    }
}
