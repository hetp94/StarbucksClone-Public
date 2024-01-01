using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StarbucksWeb.Data;
using System.Diagnostics;
using StarbucksModels.ViewModels;
using StarbucksModels.DbModels;
using Microsoft.AspNetCore.Authorization;
using StarbucksStaticDetails;


namespace StarbucksWeb.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = ApplicationRoles.App_Admin_Role)]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ProductController> _logger;
        public ProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<ProductController> logger)
        {
            _context = context;
            _environment = webHostEnvironment;
            _logger = logger;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Product.Include(p => p.Category).
                Include(p => p.Menu).Include(p => p.SubCategory)
                .OrderBy(x => x.Menu.SortingNumber).ThenBy(x => x.Category.SortingNumber).ThenBy(x => x.SubCategory.SortingNumber).ThenBy(x => x.SortingOrder);

            Debug.WriteLine("****************************************");
            Console.WriteLine("****************************************");
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.Menu)
                .Include(p => p.SubCategory)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            AddProductVM addProductVM = new();
            addProductVM.MenuList = SetMenuList();
            addProductVM.CustomizationTypeList = GetCustomizationTypeList();
            addProductVM.CustomizationAttributesList = GetCustomizationAttributesList();

            IEnumerable<string> modellist = addProductVM.CustomizationAttributesList.Where(x => x.CustomizationType == "Toppings").Select(x => x.SubCustomizationType).Distinct().ToList();

            return View(addProductVM);
        }

        public IActionResult CreateNew()
        {
            AddProductNewVM vm = new();
            vm.MenuDropDown = SetMenuList();

            vm.CustomizationCategoryDropDown = _context.CustomizationCategory.OrderBy(x => x.CategoryName).ToList().Select(i => new SelectListItem
            {
                Text = i.CategoryName,
                Value = i.CustomizationCategoryId.ToString(),
            });

            vm.CustomizationCategoryList = _context.CustomizationCategory.OrderBy(x => x.CategoryName).ToList();
            vm.CustomizationSubCategoryList = _context.CustomizationSubCategory.OrderBy(x => x.SubCategoryName).ToList();
            vm.SizeTypeList = _context.SizeType.ToList();

           // vm.CustomizationTypeList = GetCustomizationTypeList();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveProduct(AddProductNewVM GroupData)
        {
            bool result = false;
            string message = "";

            (result, message) = InsertSQL(GroupData);
            if (!result)
            {
                return Json(new { success = false, error = message });
            }

            return Json(new { success = true });
        }

        private (bool, string) InsertSQL(AddProductNewVM GroupData)
        {
            bool result;
            string message;

            (result, message) = InsertProduct(GroupData);
            if (!result)
            {
                return (false, message);
            }

            InsertCustomization(GroupData);

            InsertProdCustVisibility(GroupData);

            InsertProdCustVisibilityOptions(GroupData);

            InsertProductSizeType(GroupData);

            try
            {
                _context.SaveChanges();
                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        private (bool, string) InsertProduct(AddProductNewVM GroupData)
        {
            bool result;
            string message;
            _ = new Product();
            Product product = GroupData.Product;

            IFormFile? uploadedWholeImage = GroupData.Product.WholeImage;
            IFormFile? uploadedCroppedImage = GroupData.Product.CroppedImage;
            try
            {
                if (uploadedWholeImage != null && uploadedWholeImage.Length > 0)
                {
                    string originalFileName = uploadedWholeImage.FileName;
                    string uniqueFileName = Path.GetFileNameWithoutExtension(originalFileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(originalFileName);

                    string folder = "\\assets\\croppedImages\\";
                    string wwwRootPath = _environment.WebRootPath;

                    // folder += product.WholeImage.FileName;
                    folder += uniqueFileName;
                    folder = folder.TrimStart('\\');
                    string serverFolder = Path.Combine(wwwRootPath, folder);

                    //product.WholeImage.CopyTo(new FileStream(serverFolder, FileMode.Create));
                    using (var fileStream = new FileStream(serverFolder, FileMode.Create))
                    {
                        product.WholeImage.CopyTo(fileStream);
                    }
                    product.ImageUrl = "\\" + folder;
                }

                if (uploadedCroppedImage != null && uploadedCroppedImage.Length > 0)
                {
                    string originalFileName = uploadedCroppedImage.FileName;
                    string uniqueFileName = Path.GetFileNameWithoutExtension(originalFileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(originalFileName);

                    string folder = "\\assets\\croppedImages\\";
                    string wwwRootPath = _environment.WebRootPath;

                    // folder += product.CroppedImage.FileName;
                    folder += uniqueFileName;
                    folder = folder.TrimStart('\\');
                    string serverFolder = Path.Combine(wwwRootPath, folder);

                    //product.CroppedImage.CopyTo(new FileStream(serverFolder, FileMode.Create));
                    using (var fileStream = new FileStream(serverFolder, FileMode.Create))
                    {
                        product.CroppedImage.CopyTo(fileStream);
                    }
                    product.CroppedUrl = "\\" + folder;
                }
                _context.Product.Add(product);
                _context.SaveChanges();
                GroupData.Product.ProductId = product.ProductId;

                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        private void InsertCustomization(AddProductNewVM GroupData)
        {
            string CustomizationArray = GroupData.CustomizationArray;


            if (CustomizationArray != null || CustomizationArray != "")
            {
                List<ProductCustomization> productCustomizations = JsonConvert.DeserializeObject<List<ProductCustomization>>(CustomizationArray);
                List<ProductCustomization> productCustomizationsObj = new List<ProductCustomization>();

                foreach (var item in productCustomizations)
                {
                    productCustomizationsObj.Add(new ProductCustomization()
                    {
                        ProductId = GroupData.Product.ProductId,
                        CustomizationCategoryId = item.CustomizationCategoryId,
                        CustomizationSubcategoryId = item.CustomizationSubcategoryId,
                        CustomizationId = item.CustomizationId,
                        CustomizationOptionId = item.CustomizationOptionId,
                        Qty = item.Qty,
                    });
                }
                _context.ProductCustomization.AddRange(productCustomizationsObj);

            }
        }

        private void InsertProdCustVisibility(AddProductNewVM vm)
        {
            foreach (var prodCoursePair in vm.IsCustomizationCategorySelected)
            {
                if (prodCoursePair.Value)
                {
                    var obj = new ProdCustVisibility
                    {
                        ProductId = vm.Product.ProductId,
                        CustomizationVisibilityCode = prodCoursePair.Key
                    };
                    _context.ProdCustVisibility.Add(obj);
                }
            }
        }

        private void InsertProdCustVisibilityOptions(AddProductNewVM vm)
        {
            foreach (var prodCustOptionPair in vm.IsCustomizationSubCategorytSelected)
            {
                if (prodCustOptionPair.Value)
                {
                    var obj = new ProdCustVisibilityOptions
                    {
                        ProductId = vm.Product.ProductId,
                        CustomizationOptionId = prodCustOptionPair.Key
                    };
                    _context.ProdCustVisibilityOptions.Add(obj);
                }
            }
        }

        private void InsertProductSizeType(AddProductNewVM vm)
        {
            foreach (var prodSizePair in vm.IsSizeTypeSelected)
            {
                if (prodSizePair.Value)
                {
                    var obj = new ProductSizeType
                    {
                        ProductId = vm.Product.ProductId,
                        SizeTypeId = prodSizePair.Key
                    };
                    _context.ProductSizeType.Add(obj);
                }
            }
        }


        //private (bool, string) InsertCustomizationVisibility(AddProductNewVM GroupData)
        //{
        //    bool result;
        //    string message;

        //    CustomizationVisibility customizationVisibility = new();

        //    customizationVisibility = GroupData.CustomizationVisibility;

        //    customizationVisibility.ProductId = GroupData.Product.ProductId;

        //    customizationVisibility.AddIns1 = GroupData.AddInsArray != null ? string.Join(",", GroupData.AddInsArray) : null;
        //    customizationVisibility.BlendedOption1 = GroupData.BlendedOptionArray != null ? string.Join(",", GroupData.BlendedOptionArray) : null;
        //    customizationVisibility.ButterAndSpread1 = GroupData.ButterAndSpreadArray != null ? string.Join(",", GroupData.ButterAndSpreadArray) : null;
        //    customizationVisibility.CupOption1 = GroupData.CupOptionArray != null ? string.Join(",", GroupData.CupOptionArray) : null;
        //    customizationVisibility.EspresspAndShot1 = GroupData.EspresspAndShotArray != null ? string.Join(",", GroupData.EspresspAndShotArray) : null;
        //    customizationVisibility.BlendedOption1 = GroupData.BlendedOptionArray != null ? string.Join(",", GroupData.BlendedOptionArray) : null;
        //    customizationVisibility.Flavors1 = GroupData.FlavorsArray != null ? string.Join(",", GroupData.FlavorsArray) : null;
        //    customizationVisibility.GrindOption1 = GroupData.GrindOptionArray != null ? string.Join(",", GroupData.GrindOptionArray) : null;
        //    customizationVisibility.Ice1 = GroupData.IceArray != null ? string.Join(",", GroupData.IceArray) : null;
        //    customizationVisibility.JuiceOption1 = GroupData.JuiceOptionArray != null ? string.Join(",", GroupData.JuiceOptionArray) : null;
        //    customizationVisibility.Lemonade1 = GroupData.LemonadeArray != null ? string.Join(",", GroupData.LemonadeArray) : null;
        //    customizationVisibility.Milk1 = GroupData.MilkArray != null ? string.Join(",", GroupData.MilkArray) : null;
        //    customizationVisibility.OatmealTopping1 = GroupData.OatmealToppingArray != null ? string.Join(",", GroupData.OatmealToppingArray) : null;
        //    customizationVisibility.PreparationMethod1 = GroupData.PreparationMethodArray != null ? string.Join(",", GroupData.PreparationMethodArray) : null;
        //    customizationVisibility.SandwichOption1 = GroupData.SandwichOptionArray != null ? string.Join(",", GroupData.SandwichOptionArray) : null;
        //    customizationVisibility.StarbucksRefreshers1 = GroupData.StarbucksRefreshersArray != null ? string.Join(",", GroupData.StarbucksRefreshersArray) : null;
        //    customizationVisibility.Sweetners1 = GroupData.SweetnersArray != null ? string.Join(",", GroupData.SweetnersArray) : null;
        //    customizationVisibility.Tea1 = GroupData.TeaArray != null ? string.Join(",", GroupData.TeaArray) : null;
        //    customizationVisibility.Topping1 = GroupData.ToppingArray != null ? string.Join(",", GroupData.ToppingArray) : null;
        //    customizationVisibility.Warmed1 = GroupData.WarmedArray != null ? string.Join(",", GroupData.WarmedArray) : null;
        //    customizationVisibility.Water1 = GroupData.WaterArray != null ? string.Join(",", GroupData.WaterArray) : null;

        //    try
        //    {
        //        _context.CustomizationVisibility.Add(customizationVisibility);
        //        _context.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        return (false, ex.Message);
        //    }

        //    return (true, "");
        //}


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }
            AddProductNewVM addProductVM = new AddProductNewVM();
            addProductVM.Product = await _context.Product.FindAsync(id);
            if (addProductVM.Product == null)
            {
                return NotFound();
            }

            addProductVM.MenuDropDown = SetMenuList();
            addProductVM.CategoryList = SetCategory(addProductVM.Product.MenuId);
            addProductVM.SubCategoryList = SetSubCategory(addProductVM.Product.CategoryId);

            addProductVM.ProductCustomizationList = await _context.ProductCustomization.Where(x => x.ProductId == addProductVM.Product.ProductId).ToListAsync();






            return View(addProductVM);
        }


        public IActionResult EditNew(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }


            AddProductNewVM vm = new AddProductNewVM();

            vm.Product = _context.Product.Where(x => x.ProductId == id).FirstOrDefault();

            if (vm.Product == null)
            {
                return NotFound();
            }

            vm.MenuDropDown = SetMenuList();
            vm.CategoryList = SetCategory(vm.Product.MenuId);
            vm.SubCategoryList = SetSubCategory(vm.Product.CategoryId);


            vm.CustomizationCategoryDropDown = _context.CustomizationCategory.OrderBy(x => x.CategoryName).ToList().Select(i => new SelectListItem
            {
                Text = i.CategoryName,
                Value = i.CustomizationCategoryId.ToString(),
            });

            //svm.ProductCustomizationList = _context.ProductCustomization.Where(x => x.ProductId == id).ToList();

            var query = from productCusModel in _context.Set<ProductCustomization>().Where(x => x.ProductId == id)
                        from customizationModel in _context.Set<CustomizationCategory>().Where(x => x.CustomizationCategoryId == productCusModel.CustomizationCategoryId).DefaultIfEmpty()
                        from custSubCatModel in _context.Set<CustomizationSubCategory>().Where(x => x.CustomizationSubcategoryId == productCusModel.CustomizationSubcategoryId).DefaultIfEmpty()
                        from custModel in _context.Set<Customization>().Where(x => x.CustomizationId == productCusModel.CustomizationId).DefaultIfEmpty()
                        from custOptionModel in _context.Set<CustomizationOption>().Where(x => x.CustomizationOptionId == productCusModel.CustomizationOptionId).DefaultIfEmpty()


                        select new ProductCustomizationVM
                        {
                            ProductCustomizationKey = productCusModel.ProductCustomizationKey,

                            CustomizationCategoryId = productCusModel.CustomizationCategoryId,
                            CustomizationCategoryName = customizationModel.CategoryName,

                            CustomizationSubcategoryId = productCusModel.CustomizationSubcategoryId,
                            CustomizationSubcategoryName = custSubCatModel.SubCategoryName,

                            CustomizationId = productCusModel.CustomizationId,
                            CustomizationName = custModel.CustomizationName,

                            CustomizationOptionId = productCusModel.CustomizationOptionId,
                            CustomizationOptionName = custOptionModel.CustomizationOptionName,

                            Qty = productCusModel.Qty,


                        };

            Console.WriteLine("\n ********** ProductCustomizationVM: \n" + query.ToQueryString());

            vm.ProductCustomizationVMList = query.ToList();



            vm.CustomizationCategoryList = _context.CustomizationCategory.OrderBy(x => x.CategoryName).ToList();
            vm.CustomizationSubCategoryList = _context.CustomizationSubCategory.OrderBy(x => x.SubCategoryName).ToList();
            vm.SizeTypeList = _context.SizeType.ToList();

            //Set All Size type value to be false
            vm.IsSizeTypeSelected = _context.SizeType
                .ToDictionary(e => e.SizeTypeId, e => false);

            //find all values present in ProdSize table

            var prodSizes = _context.ProductSizeType.Where(x => x.ProductId == id).ToList();

            //map avaliable value with true in dictionary
            foreach (var prodSize in prodSizes)
            {
                if (vm.IsSizeTypeSelected.ContainsKey(prodSize.SizeTypeId))
                {
                    vm.IsSizeTypeSelected[prodSize.SizeTypeId] = true;
                }
            }


            //Set All IsCustomizationCategorySelected to be false 
            vm.IsCustomizationCategorySelected = _context.CustomizationCategory
                .ToDictionary(x => x.CustomizationCategoryId, e => false);

            //find all customization category
            var custCats = _context.ProdCustVisibility.Where(x => x.ProductId == id).ToList();

            //map available customization category with true
            foreach (var custCat in custCats)
            {
                if (vm.IsCustomizationCategorySelected.ContainsKey(custCat.CustomizationVisibilityCode))
                {
                    vm.IsCustomizationCategorySelected[custCat.CustomizationVisibilityCode] = true;
                }
            }

            //Set all IsCustomizationSubCategorytSelected to be false
            vm.IsCustomizationSubCategorytSelected = _context.CustomizationSubCategory.ToDictionary(x => x.CustomizationSubcategoryId, x => false);

            //find all available customization subcategry 
            var custSubCats = _context.ProdCustVisibilityOptions.Where(x => x.ProductId == id).ToList();

            //map
            foreach (var custSubCat in custSubCats)
            {
                if (vm.IsCustomizationSubCategorytSelected.ContainsKey(custSubCat.CustomizationOptionId))
                {
                    vm.IsCustomizationSubCategorytSelected[custSubCat.CustomizationOptionId] = true;
                }
            }


            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AddProductVM addProductVM, IFormFile? file)
        {
            if (id != addProductVM.Product.ProductId)
            {
                return NotFound();
            }

            if (addProductVM.Product.WholeImage != null)
            {
                string folder = "\\assets\\productImages\\";
                string wwwRootPath = _environment.WebRootPath;


                folder += Guid.NewGuid().ToString() + addProductVM.Product.WholeImage.FileName;
                folder = folder.TrimStart('\\');
                string serverFolder = Path.Combine(wwwRootPath, folder);

                addProductVM.Product.WholeImage.CopyTo(new FileStream(serverFolder, FileMode.Create));


                addProductVM.Product.ImageUrl = "/" + folder;
            }

            if (addProductVM.Product.CroppedImage != null)
            {
                string folder = "\\assets\\productImages\\";
                string wwwRootPath = _environment.WebRootPath;


                folder += Guid.NewGuid().ToString() + addProductVM.Product.CroppedImage.FileName;
                folder = folder.TrimStart('\\');
                string serverFolder = Path.Combine(wwwRootPath, folder);

                addProductVM.Product.CroppedImage.CopyTo(new FileStream(serverFolder, FileMode.Create));


                addProductVM.Product.CroppedUrl = "/" + folder;
            }

            try
            {
                _context.Update(addProductVM.Product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(addProductVM.Product.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateProductVM(AddProductNewVM GroupData)
        {
            bool result = false;
            string message = "";

            (result, message) = UpdateSQL(GroupData);
            if (!result)
            {
                return Json(new { success = false, error = message });
            }

            return Json(new { success = true });
        }


        private (bool, string) UpdateSQL(AddProductNewVM GroupData)
        {
            bool result;
            string message;

            (result, message) = UpdateProduct(GroupData);
            if (!result)
            {
                return (false, message);
            }

            UpdateCustomization(GroupData);
            UpdateProdCustVisibility(GroupData);
            UpdateProdCustVisibilityOptions(GroupData);
            UpdateProductSizeType(GroupData);

            try
            {
                _context.SaveChanges();
                return (true, "");
            }
            catch (Exception ex)
            {

                return (false, ex.Message);
            }
        }

        private (bool, string) UpdateProduct(AddProductNewVM GroupData)
        {
            Product? dbObj = _context.Product.FirstOrDefault(x => x.ProductId == GroupData.Product.ProductId);

            if (dbObj == null)
            {
                return (false, "Product Not Found!");
            }

            string wwwRootPath = _environment.WebRootPath;
            string? WholeImageURL = dbObj.ImageUrl;
            IFormFile? uploadedWholeImage = GroupData.Product.WholeImage;
            if (uploadedWholeImage != null && uploadedWholeImage.Length > 0)
            {
                string originalFileName = uploadedWholeImage.FileName;
                string uniqueFileName = Path.GetFileNameWithoutExtension(originalFileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(originalFileName);
                if (!string.IsNullOrEmpty(WholeImageURL))
                {
                    var oldPath = Path.Combine(wwwRootPath, WholeImageURL.TrimStart('\\'));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                        dbObj.ImageUrl = "";
                    }
                }

                // GroupData.Product.WholeImage.FileName = GroupData.Product.Name + " " + Guid.NewGuid() + ".webp";
                string folder = "\\assets\\wholeImages\\";
                // folder += GroupData.Product.WholeImage.FileName + " " + Guid.NewGuid().ToString() + ".webp";
                folder += uniqueFileName;
                folder = folder.TrimStart('\\');
                string serverFolder = Path.Combine(wwwRootPath, folder);

                //GroupData.Product.WholeImage.CopyTo(new FileStream(serverFolder, FileMode.Create));

                using (var fileStream = new FileStream(serverFolder, FileMode.Create))
                {
                    GroupData.Product.WholeImage.CopyTo(fileStream);
                }

                dbObj.ImageUrl = "\\" + folder;

            }

            string? CroppedmageURL = dbObj.CroppedUrl;

            IFormFile? uploadedCroppedImage = GroupData.Product.CroppedImage;
            if (uploadedCroppedImage != null && uploadedCroppedImage.Length > 0)
            {
                string originalFileName = uploadedCroppedImage.FileName;
                string uniqueFileName = Path.GetFileNameWithoutExtension(originalFileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(originalFileName);

                if (!string.IsNullOrEmpty(CroppedmageURL))
                {
                    try
                    {
                        var oldPath = Path.Combine(wwwRootPath, CroppedmageURL.TrimStart('\\'));
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                            dbObj.CroppedUrl = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        return (false, ex.Message);
                    }
                }

                string folder = "\\assets\\croppedImages\\";
                // folder += GroupData.Product.CroppedImage.FileName + " " + Guid.NewGuid().ToString() + ".webp";
                folder += uniqueFileName;
                folder = folder.TrimStart('\\');
                string serverFolder = Path.Combine(wwwRootPath, folder);

                using (var fileStream = new FileStream(serverFolder, FileMode.Create))
                {
                    GroupData.Product.CroppedImage.CopyTo(fileStream);
                }

                dbObj.CroppedUrl = "\\" + folder;

            }

            //Update Required Properties
            dbObj.Name = GroupData.Product.Name;
            dbObj.Descrption = GroupData.Product.Descrption;
            dbObj.RewardPoints = GroupData.Product.RewardPoints;
            dbObj.Calories = GroupData.Product.Calories;
            dbObj.NutritionInfoText = GroupData.Product.NutritionInfoText;
            dbObj.MenuId = GroupData.Product.MenuId;
            dbObj.CategoryId = GroupData.Product.CategoryId;
            dbObj.SubCategoryId = GroupData.Product.SubCategoryId;

            try
            {
                _context.Product.Update(dbObj);
                _context.SaveChanges();
                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        private void UpdateCustomization(AddProductNewVM GroupData)
        {
            string? CustomizationArray = GroupData.CustomizationArray;

            var existingRecords = _context.ProductCustomization.Where(x => x.ProductId == GroupData.Product.ProductId).ToList();

            _context.ProductCustomization.RemoveRange(existingRecords);

            if (!string.IsNullOrEmpty(CustomizationArray))
            {
                List<ProductCustomization>? productCustomizations = JsonConvert.DeserializeObject<List<ProductCustomization>>(CustomizationArray);
                List<ProductCustomization> productCustomizationsObj = new List<ProductCustomization>();
                if (productCustomizations != null)
                {
                    foreach (var item in productCustomizations)
                    {
                        productCustomizationsObj.Add(new ProductCustomization()
                        {
                            ProductId = GroupData.Product.ProductId,
                            CustomizationCategoryId = item.CustomizationCategoryId,
                            CustomizationSubcategoryId = item.CustomizationSubcategoryId,
                            CustomizationId = item.CustomizationId,
                            CustomizationOptionId = item.CustomizationOptionId,
                            Qty = item.Qty,
                        });
                    }
                    _context.ProductCustomization.AddRange(productCustomizationsObj);
                }
            }
        }

        private void UpdateProdCustVisibility(AddProductNewVM vm)
        {
            var existingRecords = _context.ProdCustVisibility.Where(x => x.ProductId == vm.Product.ProductId).ToList();

            _context.ProdCustVisibility.RemoveRange(existingRecords);

            foreach (var prodCoursePair in vm.IsCustomizationCategorySelected)
            {
                if (prodCoursePair.Value)
                {
                    var obj = new ProdCustVisibility
                    {
                        ProductId = vm.Product.ProductId,
                        CustomizationVisibilityCode = prodCoursePair.Key
                    };
                    _context.ProdCustVisibility.Add(obj);
                }
            }
        }

        private void UpdateProdCustVisibilityOptions(AddProductNewVM vm)
        {
            var existingRecords = _context.ProdCustVisibilityOptions.Where(x => x.ProductId == vm.Product.ProductId).ToList();

            _context.ProdCustVisibilityOptions.RemoveRange(existingRecords);

            foreach (var prodCustOptionPair in vm.IsCustomizationSubCategorytSelected)
            {
                if (prodCustOptionPair.Value)
                {
                    var obj = new ProdCustVisibilityOptions
                    {
                        ProductId = vm.Product.ProductId,
                        CustomizationOptionId = prodCustOptionPair.Key
                    };
                    _context.ProdCustVisibilityOptions.Add(obj);
                }
            }
        }

        private void UpdateProductSizeType(AddProductNewVM vm)
        {
            var existingRecords = _context.ProductSizeType.Where(x => x.ProductId == vm.Product.ProductId).ToList();

            _context.ProductSizeType.RemoveRange(existingRecords);

            foreach (var prodSizePair in vm.IsSizeTypeSelected)
            {
                if (prodSizePair.Value)
                {
                    var obj = new ProductSizeType
                    {
                        ProductId = vm.Product.ProductId,
                        SizeTypeId = prodSizePair.Key
                    };
                    _context.ProductSizeType.Add(obj);
                }
            }
        }

        private (bool, string) UpdateCustomizationVisibility(AddProductVM GroupData)
        {
            CustomizationVisibility? dbObj = _context.CustomizationVisibility.FirstOrDefault(x => x.ProductId == GroupData.Product.ProductId);
            if (dbObj != null)
            {
                _context.CustomizationVisibility.Remove(dbObj);
            }

            CustomizationVisibility customizationVisibility = new();

            customizationVisibility = GroupData.CustomizationVisibility;

            customizationVisibility.ProductId = GroupData.Product.ProductId;

            customizationVisibility.AddIns1 = GroupData.AddInsArray != null ? string.Join(",", GroupData.AddInsArray) : null;
            customizationVisibility.BlendedOption1 = GroupData.BlendedOptionArray != null ? string.Join(",", GroupData.BlendedOptionArray) : null;
            customizationVisibility.ButterAndSpread1 = GroupData.ButterAndSpreadArray != null ? string.Join(",", GroupData.ButterAndSpreadArray) : null;
            customizationVisibility.CupOption1 = GroupData.CupOptionArray != null ? string.Join(",", GroupData.CupOptionArray) : null;
            customizationVisibility.EspresspAndShot1 = GroupData.EspresspAndShotArray != null ? string.Join(",", GroupData.EspresspAndShotArray) : null;
            customizationVisibility.BlendedOption1 = GroupData.BlendedOptionArray != null ? string.Join(",", GroupData.BlendedOptionArray) : null;
            customizationVisibility.Flavors1 = GroupData.FlavorsArray != null ? string.Join(",", GroupData.FlavorsArray) : null;
            customizationVisibility.GrindOption1 = GroupData.GrindOptionArray != null ? string.Join(",", GroupData.GrindOptionArray) : null;
            customizationVisibility.Ice1 = GroupData.IceArray != null ? string.Join(",", GroupData.IceArray) : null;
            customizationVisibility.JuiceOption1 = GroupData.JuiceOptionArray != null ? string.Join(",", GroupData.JuiceOptionArray) : null;
            customizationVisibility.Lemonade1 = GroupData.LemonadeArray != null ? string.Join(",", GroupData.LemonadeArray) : null;
            customizationVisibility.Milk1 = GroupData.MilkArray != null ? string.Join(",", GroupData.MilkArray) : null;
            customizationVisibility.OatmealTopping1 = GroupData.OatmealToppingArray != null ? string.Join(",", GroupData.OatmealToppingArray) : null;
            customizationVisibility.PreparationMethod1 = GroupData.PreparationMethodArray != null ? string.Join(",", GroupData.PreparationMethodArray) : null;
            customizationVisibility.SandwichOption1 = GroupData.SandwichOptionArray != null ? string.Join(",", GroupData.SandwichOptionArray) : null;
            customizationVisibility.StarbucksRefreshers1 = GroupData.StarbucksRefreshersArray != null ? string.Join(",", GroupData.StarbucksRefreshersArray) : null;
            customizationVisibility.Sweetners1 = GroupData.SweetnersArray != null ? string.Join(",", GroupData.SweetnersArray) : null;
            customizationVisibility.Tea1 = GroupData.TeaArray != null ? string.Join(",", GroupData.TeaArray) : null;
            customizationVisibility.Topping1 = GroupData.ToppingArray != null ? string.Join(",", GroupData.ToppingArray) : null;
            customizationVisibility.Warmed1 = GroupData.WarmedArray != null ? string.Join(",", GroupData.WarmedArray) : null;
            customizationVisibility.Water1 = GroupData.WaterArray != null ? string.Join(",", GroupData.WaterArray) : null;

            try
            {
                _context.CustomizationVisibility.Add(customizationVisibility);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

            return (true, "");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.Menu)
                .Include(p => p.SubCategory)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }




        #region Miscellaneous

        private IEnumerable<SelectListItem> SetMenuList()
        {
            IEnumerable<SelectListItem> menuList = new List<SelectListItem>();

            menuList = _context.Menu.ToList().Select(i => new SelectListItem
            {

                Text = i.MenuName,
                Value = i.MenuId.ToString(),
            });

            return menuList;
        }

        private IEnumerable<SelectListItem> SetCategory(int menuID)
        {
            IEnumerable<SelectListItem> categoryList = new List<SelectListItem>();

            categoryList = _context.Category.Include(x => x.Menu).Where(x => x.MenuId == menuID).ToList().Select(i => new SelectListItem
            {

                Text = i.CategoryName,
                Value = i.CategoryId.ToString(),
            });

            return categoryList;
        }

        private IEnumerable<SelectListItem> SetSubCategory(int categoryId)
        {
            IEnumerable<SelectListItem> subCategoryList = new List<SelectListItem>();

            subCategoryList = _context.SubCategory.Include(x => x.Category).Where(x => x.CategoryId == categoryId).ToList().Select(i => new SelectListItem
            {

                Text = i.SubCategoryName,
                Value = i.SubcategoryId.ToString(),
            });

            return subCategoryList;
        }

        private List<CustomizationModel> GetCustomizationAttributesList()
        {
            List<CustomizationModel> customizationattributeslist = new List<CustomizationModel>();

            customizationattributeslist = _context.Customization1.Distinct().ToList();

            return customizationattributeslist;
        }

        private List<SelectListItem> GetCustomizationTypeList()
        {
            List<SelectListItem> customizationTypeList = new List<SelectListItem>();

            customizationTypeList = _context.Customization1.Select(x => x.CustomizationType).Distinct().ToList().Select(i => new SelectListItem
            {

                Text = i,
                Value = i,
            }).ToList();

            return customizationTypeList;
        }

        [HttpGet]
        public JsonResult GetCategory(string menuId)
        {
            IEnumerable<SelectListItem> categoryList = new List<SelectListItem>();
            if (menuId != null || menuId != "0")
            {
                categoryList = _context.Category.Where(x => x.MenuId == Convert.ToInt32(menuId)).ToList().Select(i => new SelectListItem
                {
                    Text = i.CategoryName,
                    Value = i.CategoryId.ToString(),
                });
            }
            return Json(categoryList);
        }

        [HttpGet]
        public JsonResult GetSubCategory(string categoryId)
        {
            IEnumerable<SelectListItem> subCategoryList = new List<SelectListItem>();
            if (categoryId != null || categoryId != "0")
            {
                subCategoryList = _context.SubCategory.Where(x => x.CategoryId == Convert.ToInt32(categoryId)).ToList().Select(i => new SelectListItem
                {
                    Text = i.SubCategoryName,
                    Value = i.SubcategoryId.ToString(),
                });
            }
            return Json(subCategoryList);
        }

        [HttpGet]
        public JsonResult GetSubCustomizationType(string customizationTypeId)
        {
            IEnumerable<SelectListItem> subCustomizationTypeId = new List<SelectListItem>();
            subCustomizationTypeId = GetSubCustomizationTypeList(customizationTypeId);
            return Json(subCustomizationTypeId);
        }

        [HttpGet]
        public JsonResult GetCustomization(string subCustomizationTypeId)
        {
            IEnumerable<SelectListItem> customizationList = new List<SelectListItem>();
            customizationList = GetCustomizationList(subCustomizationTypeId);
            return Json(customizationList);
        }

        [HttpGet]
        public JsonResult GetSubCustomization(string customizationId)
        {
            IEnumerable<SelectListItem> subCustomizationList = new List<SelectListItem>();
            subCustomizationList = GetSubCustomizationList(customizationId);
            return Json(subCustomizationList);
        }

        private IEnumerable<SelectListItem> GetSubCustomizationTypeList(string CustomizationType)
        {
            //List<string> groupedCustomerList = _context.CustomizationAttribute.Select(x=>x.CustomizationType).Distinct().ToList();



            IEnumerable<SelectListItem> customizationTypeList = new List<SelectListItem>();

            //customizationTypeList = _context.CustomizationAttribute.ToList().Select(i => new SelectListItem
            //{

            //    Text = i.CustomizationType,
            //    Value = i.CustomizationType,
            //});

            customizationTypeList = _context.Customization1.Where(y => y.CustomizationType == CustomizationType).Select(x => x.SubCustomizationType).Distinct().ToList().Select(i => new SelectListItem
            {

                Text = i,
                Value = i,
            });

            return customizationTypeList;
        }

        private IEnumerable<SelectListItem> GetCustomizationList(string SubCustomizationType)
        {
            //List<string> groupedCustomerList = _context.CustomizationAttribute.Select(x=>x.CustomizationType).Distinct().ToList();



            IEnumerable<SelectListItem> customizationTypeList = new List<SelectListItem>();

            //customizationTypeList = _context.CustomizationAttribute.ToList().Select(i => new SelectListItem
            //{

            //    Text = i.CustomizationType,
            //    Value = i.CustomizationType,
            //});

            customizationTypeList = _context.Customization1.Where(y => y.SubCustomizationType == SubCustomizationType).Select(x => x.Customization).Distinct().ToList().Select(i => new SelectListItem
            {
                Text = i,
                Value = i,
            });

            return customizationTypeList;
        }

        private IEnumerable<SelectListItem> GetSubCustomizationList(string Customization)
        {
            //List<string> groupedCustomerList = _context.CustomizationAttribute.Select(x=>x.CustomizationType).Distinct().ToList();



            IEnumerable<SelectListItem> customizationTypeList = new List<SelectListItem>();

            //customizationTypeList = _context.CustomizationAttribute.ToList().Select(i => new SelectListItem
            //{

            //    Text = i.CustomizationType,
            //    Value = i.CustomizationType,
            //});

            customizationTypeList = _context.Customization1.Where(y => y.Customization == Customization).Select(x => x.SubCustomization).Distinct().ToList().Select(i => new SelectListItem
            {
                Text = i,
                Value = i,
            });

            return customizationTypeList;
        }


        #endregion



        public IActionResult IndexNew()
        {
            _logger.LogInformation("****************************************");
            var applicationDbContext = _context.Product.Include(p => p.Category).
                Include(p => p.Menu).Include(p => p.SubCategory)
                .OrderBy(x => x.Menu.SortingNumber).ThenBy(x => x.Category.SortingNumber).ThenBy(x => x.SubCategory.SortingNumber).ThenBy(x => x.SortingOrder);
            return View(applicationDbContext.ToList());
        }



        [HttpGet]
        public JsonResult GetCustomizationSubcategoryList(string CustomizationCategoryId)
        {
            IEnumerable<SelectListItem> subCategoryList = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(CustomizationCategoryId))
            {
                subCategoryList = _context.CustomizationSubCategory.Where(x => x.CustomizationCategoryId == Convert.ToInt32(CustomizationCategoryId)).ToList().Select(i => new SelectListItem
                {
                    Text = i.SubCategoryName,
                    Value = i.CustomizationSubcategoryId.ToString(),
                });
            }
            return Json(subCategoryList);
        }

        [HttpGet]
        public JsonResult GetCustomizationDropDown(string CustomizationSubCategoryId)
        {
            IEnumerable<SelectListItem> list = new List<SelectListItem>();
            Console.WriteLine("CustomizationSubCategoryId: " + CustomizationSubCategoryId);
            if (!string.IsNullOrEmpty(CustomizationSubCategoryId))
            {
                list = _context.Customization.Where(x => x.CustomizationSubcategoryId == Convert.ToInt32(CustomizationSubCategoryId)).ToList().Select(i => new SelectListItem
                {
                    Text = i.CustomizationName,
                    Value = i.CustomizationId.ToString(),
                });
            }
            return Json(list);
        }

        [HttpGet]
        public JsonResult GetCustomizationOptionsList(string CustomizationId)
        {
            IEnumerable<SelectListItem> list = new List<SelectListItem>();
            Console.WriteLine("CustomizationId: " + CustomizationId);
            if (!string.IsNullOrEmpty(CustomizationId))
            {
                list = _context.CustomizationOption.Where(x => x.CustomizationId == Convert.ToInt32(CustomizationId)).ToList().Select(i => new SelectListItem
                {
                    Text = i.CustomizationOptionName,
                    Value = i.CustomizationOptionId.ToString(),
                });
            }
            return Json(list);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNew(AddProductNewVM vm)
        {
            if (ModelState.IsValid)
            {
                _context.Product.Add(vm.Product);

                _context.SaveChanges();


                foreach (var prodCoursePair in vm.IsCustomizationCategorySelected)
                {
                    if (prodCoursePair.Value)
                    {
                        var obj = new ProdCustVisibility
                        {
                            ProductId = vm.Product.ProductId,
                            CustomizationVisibilityCode = prodCoursePair.Key
                        };
                        _context.ProdCustVisibility.Add(obj);
                    }
                }

                foreach (var prodCustOptionPair in vm.IsCustomizationSubCategorytSelected)
                {
                    if (prodCustOptionPair.Value)
                    {
                        var obj = new ProdCustVisibilityOptions
                        {
                            ProductId = vm.Product.ProductId,
                            CustomizationOptionId = prodCustOptionPair.Key
                        };
                        _context.ProdCustVisibilityOptions.Add(obj);
                    }
                }


                foreach (var prodSizePair in vm.IsSizeTypeSelected)
                {
                    if (prodSizePair.Value)
                    {
                        var obj = new ProductSizeType
                        {
                            ProductId = vm.Product.ProductId,
                            SizeTypeId = prodSizePair.Key
                        };
                        _context.ProductSizeType.Add(obj);
                    }
                }


                _context.SaveChanges();



            }
            vm.CustomizationCategoryList = _context.CustomizationCategory.OrderBy(x => x.CategoryName).ToList();
            vm.CustomizationSubCategoryList = _context.CustomizationSubCategory.OrderBy(x => x.SubCategoryName).ToList();

            vm.MenuDropDown = SetMenuList();
            vm.CustomizationTypeList = GetCustomizationTypeList();
            //vm.CustomizationAttributesList = GetCustomizationAttributesList();
            return View(vm);
        }



    }
}
