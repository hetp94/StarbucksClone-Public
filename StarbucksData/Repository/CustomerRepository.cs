using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StarbucksData.IRepository;
using StarbucksModels;
using StarbucksModels.DbModels;
using StarbucksModels.ViewModels;
using StarbucksStaticDetails;
using StarbucksWeb.Data;
using System.Linq;
using System.Linq.Expressions;

namespace StarbucksData.Repository
{
    public class CustomerRepository : Repository<DummyClass>, ICustomerRepository
    {
        private ApplicationDbContext _context;
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void AddOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetail.Add(orderDetail);
        }

        public void AddOrderHeader(OrderHeader orderHeader)
        {
            _context.OrderHeader.Add(orderHeader);
        }

        public async Task<CartVM> GetCartAsync(List<ListofProductIds> listofProductIds, bool Autheticated)
        {
            CartVM cartVM = new();

            List<Product> products = new List<Product>();

            foreach (var item in listofProductIds)
            {
                Product product = await _context.Product.Where(x => x.ProductId == item.ProductId).FirstOrDefaultAsync();

                if (product != null)
                {
                    products.Add(product);
                }
                if (!Autheticated)
                {
                    products.ForEach(product => product.ItemPrice = 0);
                }

                cartVM.ProductList = products;

                if (Autheticated) //if user is not autheticated, we are not showing prices
                {
                    if (products != null)
                    {
                        cartVM.TotalPrice = products.Sum(x => x.ItemPrice);
                    }
                }
            }
            return cartVM;
        }

        public async Task<OrderHeader> GetFirstOrDefaultOrderHeaderAsync(Expression<Func<OrderHeader, bool>> filter)
        {
            return await _context.OrderHeader.Where(filter).FirstOrDefaultAsync();
        }

        public async Task<MenuVM> GetMenuAsync()
        {
            MenuVM menuVM = new();
            menuVM.MenuList = await _context.Menu.Where(x => x.ShowVisibility == true).OrderBy(x => x.SortingNumber).ToListAsync();
            menuVM.CategoryList = await _context.Category.OrderBy(x => x.SortingNumber).ToListAsync();
            menuVM.ProductList = await _context.Product.Include(x => x.SubCategory).OrderBy(x => x.SortingOrder).ToListAsync();
            return menuVM;
        }

        public async Task<MenuVM?> GetMenuAsync(string CategoryName)
        {
            int id = await _context.Category.Where(x => x.CategoryName == CategoryName).Select(x => x.CategoryId).FirstOrDefaultAsync();

            if (id == 0)
            {
                return null;
            }

            MenuVM menuVM = new();
            menuVM.ActiveCategory = CategoryName;
            menuVM.MenuList = await _context.Menu.Where(x => x.ShowVisibility == true).OrderBy(x => x.SortingNumber).ToListAsync();
            menuVM.CategoryList = await _context.Category.OrderBy(x => x.SortingNumber).ToListAsync();
            menuVM.subCategoryList = await _context.SubCategory.Where(x => x.CategoryId == id).OrderBy(x => x.SortingNumber).ToListAsync();
            menuVM.ProductList = await _context.Product.Where(x => x.CategoryId == id).OrderBy(x => x.SortingOrder).ToListAsync();

            return menuVM;
        }

        public async Task<IEnumerable<OrderHistoryVM>> GetOrderHistory(string ApplicationUserId, string FirstName, bool IsTestCustomer)
        {
            var query = (from orderHeader in _context.Set<OrderHeader>().Where(x => x.PaymentStatus == StaticDetails.PaymentStatusPaid).Where(x => x.ApplicationUserId == ApplicationUserId)
                         from orderDetail in _context.Set<OrderDetail>().Where(x => x.OrderId == orderHeader.Id)
                         from product in _context.Set<Product>().Where(x => x.ProductId == orderDetail.ProductId)

                         select new OrderHistoryVM
                         {
                             ProductName = product.Name,
                             Price = orderDetail.Price,
                             CroppedUrl = product.CroppedUrl,
                             FirstName = FirstName,
                             OrderDetailId = orderDetail.OrderDetailId,
                             OrderHeaderId = orderHeader.Id,
                             OrderDate = orderHeader.OrderDate
                         });

            if (IsTestCustomer)
            {
                query = query.Take(10).OrderByDescending(x => x.OrderDetailId);
            }
            else
            {
                query = query.Take(25).OrderByDescending(x => x.OrderDetailId);
            }

            await Console.Out.WriteLineAsync("Query: " + query.ToQueryString());

            return await query.ToListAsync();
        }

        public async Task<ProductVM?> GetProductAsync(int id)
        {
            //var idPara = new SqlParameter("@id", id);

            //var product = _context.Product.FromSqlRaw("EXECUTE [dbo].[GetProduct]  @id", idPara)

            //    .AsEnumerable().ToList();

            ProductVM productVM = new();

            productVM.Product = _context.Product.Include(x => x.Category).FirstOrDefault(x => x.ProductId == id);

            if (productVM.Product == null)
            {
                return null;
            }

            productVM.CategoryName = productVM.Product.Category.CategoryName;

            var query = (from productCusModel in _context.Set<ProductCustomization>().Where(x => x.ProductId == id)
                         from customizationModel in _context.Set<CustomizationCategory>().Where(x => x.CustomizationCategoryId == productCusModel.CustomizationCategoryId)
                         from custSubCatModel in _context.Set<CustomizationSubCategory>().Where(x => x.CustomizationSubcategoryId == productCusModel.CustomizationSubcategoryId)
                         from custModel in _context.Set<Customization>().Where(x => x.CustomizationId == productCusModel.CustomizationId)
                         from custOptionModel in _context.Set<CustomizationOption>().Where(x => x.CustomizationOptionId == productCusModel.CustomizationOptionId)
                         from secondCustOption in _context.Set<CustomizationOption>()
                         where (custModel.CustomizationId == secondCustOption.CustomizationId)

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

                             CustomizationOptionId2 = secondCustOption.CustomizationOptionId,
                             CustomizationOptionName2 = secondCustOption.CustomizationOptionName,

                             Qty = productCusModel.Qty,
                         });

            Console.WriteLine("\n ********** ProductCustomizationVM: \n" + query.ToQueryString());


            productVM.productCustomizationVMList = await query.ToListAsync();

            productVM.productSizeTypes = await _context.ProductSizeType.Include(x => x.Product).Include(x => x.SizeType).Where(x => x.ProductId == id).ToListAsync();

            return productVM;
        }

        public void UpdateStripeSession(int OrderId, string sessionId, string paymentIntentId)
        {
            var orderHeaderFromDb = _context.OrderHeader.FirstOrDefault(x => x.Id == OrderId);
            orderHeaderFromDb.SessionId = sessionId;
            orderHeaderFromDb.PaymentIntentId = paymentIntentId;
        }

        public void UpdateStripeSessionToPaid(int OrderId, string paymentIntentId, string PaymentStatusPaid, string StatusApproved)
        {
            var orderHeaderFromDb = _context.OrderHeader.FirstOrDefault(x => x.Id == OrderId);

            orderHeaderFromDb.PaymentIntentId = paymentIntentId;
            orderHeaderFromDb.PaymentStatus = PaymentStatusPaid;
            orderHeaderFromDb.OrderStatus = StatusApproved;
            orderHeaderFromDb.Dt_Rec_Modified = DateTime.Now;
        }
    }
}
