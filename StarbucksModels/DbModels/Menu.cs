using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace StarbucksModels.DbModels
{
    public class Menu
    {
        [Key]
        public int MenuId { get; set; }
        [Required(ErrorMessage = "Menu Type is Required")]
        [MaxLength(50)]
        [DisplayName("Menu Name")]
        public string MenuName { get; set; }
        [Required]
        [DisplayName("Visible")]
        public bool ShowVisibility { get; set; } = false;
        public int SortingNumber { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime EffectiveDate { get; set; } = DateTime.Now;
    }
}
