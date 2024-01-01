using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StarbucksData.IRepository;
using StarbucksModels.DbModels;
using StarbucksModels.ViewModels;
using StarbucksStaticDetails;

namespace StarbucksWeb.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = ApplicationRoles.App_Admin_Role)]
    public class LayoutController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public LayoutController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            UIDetail.ActiveController = "Layout";
            LayoutVM layoutVM = new();
            var menus = await _unitOfWork.AdminRepo.GetAllMenuListAsync();
            layoutVM.MenuList = menus;
            layoutVM.MenuDropDown = menus.Select(i => new SelectListItem
            {

                Text = i.MenuName,
                Value = i.MenuId.ToString(),
            });


            return View(layoutVM);
        }

        #region Update Layout

        public async Task<JsonResult> UpdateMenuItem(string itemIds)
        {
            int count = 1;
            List<int> itemIdList = new();
            itemIdList = itemIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            if (itemIdList.Count > 0)
            {
                foreach (var item in itemIdList)
                {
                    try
                    {
                        Menu dbObj = await _unitOfWork.AdminRepo.GetFirstOrDefaultMenuAsync(x => x.MenuId == item);
                        dbObj.SortingNumber = count;
                        _unitOfWork.AdminRepo.UpdateMenu(dbObj);
                    }

                    catch (Exception)
                    {
                        continue;
                    }
                    count++;
                }
                await _unitOfWork.SaveAsync();
            }

            return Json(true);
        }

        public async Task<JsonResult> UpdateCategoryItem(string itemIds)
        {
            int count = 1;
            List<int> itemIdList = new();
            itemIdList = itemIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            if (itemIdList.Count > 0)
            {
                foreach (var item in itemIdList)
                {
                    try
                    {
                        Category dbObj = await _unitOfWork.AdminRepo.GetFirstOrDefaultCategoryAsync(x => x.CategoryId == item);
                        dbObj.SortingNumber = count;
                        _unitOfWork.AdminRepo.UpdateCategory(dbObj);
                    }

                    catch (Exception)
                    {
                        continue;
                    }
                    count++;
                }
                await _unitOfWork.SaveAsync();
            }

            return Json(true);
        }

        public async Task<JsonResult> UpdateSubCategoryItem(string itemIds)
        {
            int count = 1;
            List<int> itemIdList = new();
            itemIdList = itemIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            if (itemIdList.Count > 0)
            {
                foreach (var item in itemIdList)
                {
                    try
                    {
                        SubCategory subCategoryItem = await _unitOfWork.AdminRepo.GetFirstOrDefaultSubCategoryAsync(x => x.SubcategoryId == item);
                        subCategoryItem.SortingNumber = count;
                        _unitOfWork.AdminRepo.UpdateSubCategory(subCategoryItem);
                    }

                    catch (Exception)
                    {
                        continue;
                    }
                    count++;
                }
                await _unitOfWork.SaveAsync();
            }

            return Json(true);
        }

        public async Task<JsonResult> UpdateProductItem(string itemIds)
        {
            int count = 1;
            List<int> itemIdList = new();
            itemIdList = itemIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            if (itemIdList.Count > 0)
            {
                foreach (var item in itemIdList)
                {
                    try
                    {
                        Product productItem = await _unitOfWork.AdminRepo.GetFirstOrDefaultProductAsync(x => x.ProductId == item);
                        if (productItem != null)
                        {
                            productItem.SortingOrder = count;
                            _unitOfWork.AdminRepo.UpdateProduct(productItem);
                        }
                    }

                    catch (Exception)
                    {
                        continue;
                    }
                    count++;
                }
                await _unitOfWork.SaveAsync();
            }

            return Json(true);
        }
        #endregion





        #region Miscellaneous


        [HttpGet]
        public async Task<JsonResult> GetCategory(string menuId)
        {
            Console.WriteLine("Menu Id: " + menuId);
            IEnumerable<SelectListItem> categoryList = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(menuId))
            {
                var cateList = await _unitOfWork.AdminRepo.GetAllCategoryListAsync(x => x.MenuId == Convert.ToInt32(menuId));
                categoryList = cateList.Select(i => new SelectListItem
                {
                    Text = i.CategoryName,
                    Value = i.CategoryId.ToString(),
                });
            }
            return Json(categoryList);
        }

        [HttpGet]
        public async Task<JsonResult> GetSubCategory(string categoryId)
        {
            IEnumerable<SelectListItem> subCategoryList = new List<SelectListItem>();
            if (categoryId != null || categoryId != "0")
            {
                var list = await _unitOfWork.AdminRepo.GetAllSubCategoryListAsync(x => x.CategoryId == Convert.ToInt32(categoryId));
                subCategoryList = list.Select(i => new SelectListItem
                {
                    Text = i.SubCategoryName,
                    Value = i.SubcategoryId.ToString(),
                });
            }
            return Json(subCategoryList);
        }

        [HttpGet]
        public async Task<JsonResult> GetProduct(string menuId, string categoryId, string subcategoryId)
        {
            Console.WriteLine("Product menu ID: " + menuId);
            IEnumerable<SelectListItem> productList = new List<SelectListItem>();
            int menuIdValue = Convert.ToInt32(menuId);
            int categoryIdValue = Convert.ToInt32(categoryId);
            int subcategoryIdValue = Convert.ToInt32(subcategoryId);
            if (categoryId != null || categoryId != "0")
            {
                var list = await _unitOfWork.AdminRepo.GetAllProductListAsync(); // _context.Product.Where(x => x.MenuId == menuIdValue).
                productList = list.Where(x => x.MenuId == menuIdValue).
                    Where(x => x.CategoryId == categoryIdValue).Where(x => x.SubCategoryId == subcategoryIdValue).ToList().Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.ProductId.ToString(),
                    });
            }
            return Json(productList);
        }

        private async Task<IEnumerable<SelectListItem>> SetMenuListDropDown()
        {
            IEnumerable<SelectListItem> menuList = new List<SelectListItem>();
            var menus = await _unitOfWork.AdminRepo.GetAllMenuListAsync();
            menuList = menus.Select(i => new SelectListItem
            {

                Text = i.MenuName,
                Value = i.MenuId.ToString(),
            });

            return menuList;
        }

        private async Task<IEnumerable<SelectListItem>> SetCategoryDropDown(int menuID)
        {
            IEnumerable<SelectListItem> categoryList = new List<SelectListItem>();
            var list = await _unitOfWork.AdminRepo.GetAllCategoryListAsync(x => x.MenuId == menuID);
            categoryList = list.Select(i => new SelectListItem
            {

                Text = i.CategoryName,
                Value = i.CategoryId.ToString(),
            });

            return categoryList;
        }

        private async Task<IEnumerable<SelectListItem>> SetSubCategoryDropDown(int categoryId)
        {
            IEnumerable<SelectListItem> subCategoryList = new List<SelectListItem>();
            var list = await _unitOfWork.AdminRepo.GetAllSubCategoryListAsync(x => x.CategoryId == categoryId);
            subCategoryList = list.Select(i => new SelectListItem
            {

                Text = i.SubCategoryName,
                Value = i.SubcategoryId.ToString(),
            });

            return subCategoryList;
        }

        #endregion
    }
}
