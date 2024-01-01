

using StarbucksModels.DbModels;

namespace StarbucksModels.ViewModels
{
    public class MenuVM
    {
        public string ActiveCategory { get; set; } = null!;
        public IEnumerable<Menu> MenuList { get; set; } = null!;
        public IEnumerable<Category> CategoryList { get; set; } = null!;
        public IEnumerable<SubCategory> subCategoryList { get; set; } = null!;
        public IEnumerable<Product> ProductList { get; set; } = null!;
    }
}
