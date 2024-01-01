using Microsoft.AspNetCore.Mvc.Rendering;
using StarbucksModels.DbModels;

namespace StarbucksModels.ViewModels
{
    public class AddProductVM
    {
        public Product Product { get; set; }
        public ProductCustomization ProductCustomization { get; set; }
        public IEnumerable<ProductCustomization> productCustomizationsList { get; set; }

        public IEnumerable<SelectListItem> MenuList { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> SubCategoryList { get; set; }
        public List<SelectListItem> CustomizationTypeList { get; set; }
        public IEnumerable<SelectListItem> SubCustomizationTypeList { get; set; }
        public IEnumerable<SelectListItem> CustomizationList { get; set; }
        public IEnumerable<SelectListItem> SubCustomizationList { get; set; }

        public string CustomizationArray { get; set; }

        public CustomizationVisibility CustomizationVisibility { get; set; }
        public List<string> Labels { get; set; }
        public string[] AddInsArray { get; set; }
        public string[] BlendedOptionArray { get; set; }
        public string[] ButterAndSpreadArray { get; set; }
        public string[] CupOptionArray { get; set; }
        public string[] EspresspAndShotArray { get; set; }
        public string[] FlavorsArray { get; set; }
        public string[] GrindOptionArray { get; set; }
        public string[] IceArray { get; set; }
        public string[] JuiceOptionArray { get; set; }
        public string[] LemonadeArray { get; set; }
        public string[] MilkArray { get; set; }
        public string[] OatmealToppingArray { get; set; }
        public string[] PreparationMethodArray { get; set; }
        public string[] SandwichOptionArray { get; set; }

        public string[] StarbucksRefreshersArray { get; set; }
        public string[] SweetnersArray { get; set; }
        public string[] TeaArray { get; set; }
        public string[] ToppingArray { get; set; }
        public string[] WarmedArray { get; set; }
        public string[] WaterArray { get; set; }

        public IEnumerable<CustomizationModel> CustomizationAttributesList { get; set; }
    }
}
