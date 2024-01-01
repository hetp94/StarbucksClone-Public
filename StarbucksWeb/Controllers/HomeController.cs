using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarbucksModels.DbModels;
using StarbucksStaticDetails;
using StarbucksWeb.Data;
using StarbucksWeb.Models;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;

namespace StarbucksWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            UIDetail.ActiveController = "Home";

            string encodedValue = System.Net.WebUtility.UrlEncode("raw-string-text");
            Console.WriteLine("encodedValue: " + encodedValue);
            string decodedValue = System.Net.WebUtility.UrlDecode(encodedValue);
            Console.WriteLine("decodedValue: " + decodedValue);

            //To Add Products From Excel
            //LoadProductInfo();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private static DataTable ProductTable = new DataTable();
        private string ProductExcelDirectory = "C:\\Users\\Het\\Desktop\\Starbucks\\Test Folder\\Product Info.xlsx";



        #region Miscellaneous


        private void LoadProductInfo()
        {
            string fileName = ProductExcelDirectory;
            using (var excelWorkbook = new XLWorkbook(fileName))
            {
                IXLRows nonEmptyDataRows = excelWorkbook.Worksheet(1).RowsUsed();
                int number = nonEmptyDataRows.Count();
                Console.WriteLine("Row Count: " + nonEmptyDataRows.Count());

                List<Product> products = new List<Product>();

                foreach (var dataRow in nonEmptyDataRows.Skip(1))
                {
                    Product product = new Product();

                    string Name = dataRow.Cell(4).Value.ToString();
                    Console.WriteLine("Name: " + Name);

                    string Descrption = dataRow.Cell(5).Value.ToString();
                    Console.WriteLine("Descrption: " + Descrption);

                    string RewardPoints = dataRow.Cell(7).Value.ToString();
                    Console.WriteLine("RewardPoints: " + RewardPoints);

                    string NutritionInfoText = dataRow.Cell(8).Value.ToString();
                    Console.WriteLine("NutritionInfoText: " + NutritionInfoText);

                    string MenuId = dataRow.Cell(1).Value.ToString();
                    Console.WriteLine("MenuId: " + MenuId);

                    string CategoryId = dataRow.Cell(2).Value.ToString();
                    Console.WriteLine("CategoryId: " + CategoryId);

                    string SubCategoryId = dataRow.Cell(3).Value.ToString();
                    Console.WriteLine("SubCategoryId: " + SubCategoryId);

                    string ImageUrl = "\\assets\\wholeImages\\" + dataRow.Cell(9).Value.ToString();
                    Console.WriteLine("ImageUrl: " + ImageUrl);

                    string CroppedUrl = "\\assets\\croppedImages\\" + dataRow.Cell(10).Value.ToString();
                    Console.WriteLine("CroppedUrl: " + CroppedUrl);

                    Console.WriteLine("******");
                    Console.WriteLine("");



                    product.Name = dataRow.Cell(4).Value.ToString();
                    product.Descrption = dataRow.Cell(5).Value.ToString();
                    product.RewardPoints = Convert.ToInt32(dataRow.Cell(7).Value.ToString());
                    product.NutritionInfoText = dataRow.Cell(8).Value.ToString();
                    product.MenuId = ReturnMenuIdFromString(dataRow.Cell(1).Value.ToString());
                    product.CategoryId = ReturnCategoryIdFromString(dataRow.Cell(1).Value.ToString(), dataRow.Cell(2).Value.ToString());
                    product.SubCategoryId = ReturnSubCategoryIdFromString(dataRow.Cell(2).Value.ToString(), dataRow.Cell(3).Value.ToString());

                    product.Calories = Convert.ToInt32(dataRow.Cell(6).Value.ToString());
                    product.SortingOrder = 1;
                    product.ImageUrl = ImageUrl;
                    product.CroppedUrl = CroppedUrl;
                    products.Add(product);

                }

                //_context.Product.AddRange(products);
                //_context.SaveChanges();

            }
        }

        private int ReturnMenuIdFromString(string menuName)
        {
            int menuId = 0;

            menuId = _context.Menu.Where(x => x.MenuName == menuName).Select(x => x.MenuId).FirstOrDefault();

            Console.WriteLine("Menu Name: " + menuName + " Menud ID: " + menuId);
            if (menuId == 0 || menuId == null)
            {
                throw new Exception();
            }

            Console.WriteLine("*");

            return menuId;
        }

        private int ReturnCategoryIdFromString(string menuName, string categoryName)
        {
            int categoryId = 0;

            categoryId = _context.Category.Include(x => x.Menu).Where(x => x.Menu.MenuName == menuName).Where(x=>x.CategoryName == categoryName).Select(x => x.CategoryId).FirstOrDefault();

            Console.WriteLine("Category Name: " + categoryName + " Category ID: " + categoryId);
            if (categoryId == 0 || categoryId == null)
            {
                throw new Exception();
            }

            Console.WriteLine("**");

            return categoryId;
        }

        private int ReturnSubCategoryIdFromString(string categoryName, string subCategory)
        {
            int subCategoryId = 0;

            subCategoryId = _context.SubCategory.Include(x => x.Category).Where(x => x.Category.CategoryName == categoryName).
                Where(x=>x.SubCategoryName == subCategory).Select(x => x.SubcategoryId).FirstOrDefault();

            Console.WriteLine("Sub Category Name: " + subCategory + " Sub Category ID: " + subCategoryId);
            if (subCategoryId == 0 || subCategoryId == null)
            {
                throw new Exception();
            }

            Console.WriteLine("***");

            return subCategoryId;
        }


        #endregion

    }
}