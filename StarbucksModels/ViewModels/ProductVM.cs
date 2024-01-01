using StarbucksModels.DbModels;

namespace StarbucksModels.ViewModels
{
    public class ProductVM
    {
        public string CategoryName { get; set; } = null!;
        public Product Product { get; set; } = null!;

        public IEnumerable<ProductCustomizationVM> productCustomizationVMList { get; set; } = null!;
      
        public IEnumerable<ProductSizeType> productSizeTypes { get; set; } = null!;
    }
}
