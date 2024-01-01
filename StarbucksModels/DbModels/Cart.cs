using System.ComponentModel.DataAnnotations;

namespace StarbucksModels.DbModels
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public int OrderId { get; set; }
        [Required]
        [MaxLength(128)]
        public string ProductName { get; set; }
        [Required]
        public int Qty { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [MaxLength(50)]
        public string? Size { get; set; }
    }
}
