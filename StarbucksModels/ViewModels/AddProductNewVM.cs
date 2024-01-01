using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using StarbucksModels.DbModels;

namespace StarbucksModels.ViewModels
{
    public class AddProductNewVM
    {
        public Product Product { get; set; }

       


        public ProductCustomization ProductCustomization { get; set; }

        public IEnumerable<ProductCustomization> ProductCustomizationList { get; set; }

        public IEnumerable<ProductCustomizationVM> ProductCustomizationVMList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> MenuDropDown { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> SubCategoryList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CustomizationCategoryDropDown { get; set; }


        [ValidateNever]
        public List<CustomizationCategory> CustomizationCategoryList { get; set; }

        public Dictionary<int, bool> IsCustomizationCategorySelected { get; set; }

        [ValidateNever]
        public List<CustomizationSubCategory> CustomizationSubCategoryList { get; set; }
        public Dictionary<int, bool> IsCustomizationSubCategorytSelected { get; set; }


        [ValidateNever]
        public List<SizeType> SizeTypeList { get; set; }
        public Dictionary<int, bool> IsSizeTypeSelected { get; set; }


        [ValidateNever]
        public List<SelectListItem> CustomizationTypeList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> SubCustomizationTypeList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CustomizationList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> SubCustomizationList { get; set; }


        public string? CustomizationArray { get; set; }



    }
}
