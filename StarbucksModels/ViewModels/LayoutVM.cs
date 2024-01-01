using Microsoft.AspNetCore.Mvc.Rendering;
using StarbucksModels.DbModels;

namespace StarbucksModels.ViewModels
{
    public class LayoutVM
    {

        public IEnumerable<Menu> MenuList { get; set; }

        public IEnumerable<SelectListItem> MenuDropDown { get; set; }
        public List<Category> CategoryList { get; set; }
    }
}
