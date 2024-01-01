using StarbucksModels.DbModels;
using System.ComponentModel.DataAnnotations;

namespace StarbucksModels.ViewModels
{
    public class CartVM
    {
        public IEnumerable<Product> ProductList { get; set; }
        [Required]
        public string ProductId { get; set; }
        public string ProductName { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
