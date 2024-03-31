using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StarbucksData.IRepository;
using StarbucksModels.DbModels;
using StarbucksModels.ViewModels;
using StarbucksStaticDetails;
using StarbucksWeb.Data;
using Stripe.Checkout;

namespace StarbucksWeb.Areas.Customer.Controllers
{
    [Area(nameof(Customer))]
    public class MenuController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private IConfiguration _configuration;

        public MenuController(IUnitOfWork unitOfWork,  IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.CustomerRepo.GetMenuAsync());
        }

        public async Task<IActionResult> drinks(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return NotFound();
            }

            MenuVM vm = await _unitOfWork.CustomerRepo.GetMenuAsync(name);
            if (vm == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        public async Task<IActionResult> Product(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            ProductVM vm = await _unitOfWork.CustomerRepo.GetProductAsync(id);

            if (vm == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        public IActionResult Cart()
        {
            return View();
        }

        public async Task<JsonResult> CartData([FromBody] List<ListofProductIds> dataObj)
        {
            if (ModelState.IsValid)
            {
                CartVM cartVM = new CartVM();

                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    cartVM = await _unitOfWork.CustomerRepo.GetCartAsync(dataObj, true);

                    return Json(cartVM);
                }
                else
                {
                    cartVM = await _unitOfWork.CustomerRepo.GetCartAsync(dataObj, false);

                    return Json(cartVM);
                }
            }

            return Json("");
        }

        [Authorize]
        public IActionResult Continue()
        {
            return RedirectToAction(nameof(Cart));
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CheckOut1([FromBody] List<ListofProductIds> VM)
        {
            if (ModelState.IsValid)
            {
                List<int> IdList = VM.Select(x => x.ProductId).ToList();

                string combinedString = string.Join(",", IdList.ToArray());

                if (!string.IsNullOrEmpty(combinedString))
                {
                    HttpContext.Session.SetString("ProductIds", combinedString);
                    return Json(true);
                }

                return Json("Please add items to the cart!");
            }
            else
            {
                return Json(false);
            }
        }

        [Authorize]
        public async Task<IActionResult> CheckOut()
        {
            string ids = HttpContext.Session.GetString("ProductIds");

            if (string.IsNullOrEmpty(ids))
            {
                return RedirectToAction(nameof(Cart));
            }

            var user = await _userManager.GetUserAsync(User);
            var applicationUserId = user.Id;

            List<string> idList = ids.Split(',').ToList();
            List<Product> products = new List<Product>();

            foreach (var item in idList)
            {
                Product product = await _unitOfWork.AdminRepo.GetFirstOrDefaultProductAsync(x => x.ProductId == Convert.ToInt32(item));

                if (product != null)
                {
                    //product.ItemPrice = Math.Round(product.ItemPrice * 1.0975m, 2) ; //add the tax here @ 9.75%
                    products.Add(product);
                }
            }

            OrderHeader orderHeader = new OrderHeader()
            {
                ApplicationUserId = applicationUserId,
                OrderTotal = products.Sum(x => x.ItemPrice),
                PaymentStatus = StaticDetails.PaymentStatusPending,
                OrderStatus = StaticDetails.StatusPending,
            };


            _unitOfWork.CustomerRepo.AddOrderHeader(orderHeader);

            try
            {
                _unitOfWork.Save();
            }
            catch (Exception)
            {
                throw;
            }

            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (var product in products)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    OrderId = orderHeader.Id,
                    ProductId = product.ProductId,
                    Price = product.ItemPrice,
                };
                _unitOfWork.CustomerRepo.AddOrderDetail(orderDetail);
            }

            try
            {
                _unitOfWork.Save();
            }
            catch (Exception)
            {
                throw;
            }


            // Stripe Details
            // var domain = "https://localhost:44322/";
            var domain = _configuration.GetValue<string>("Stripe:DomainURL");


            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"customer/menu/OrderConformation?id={orderHeader.Id}",
                CancelUrl = domain + "customer/menu/cart",
            };

            //products.ForEach(product => product.ItemPrice = product.ItemPrice * 1.0975m);

            foreach (Product product2 in products)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = (long)(product2.ItemPrice * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product2.Name,
                        }
                    },
                    Quantity = 1,
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            _unitOfWork.CustomerRepo.UpdateStripeSession(orderHeader.Id, session.Id, session.PaymentIntentId);

            try
            {
                _unitOfWork.Save();
            }
            catch (Exception)
            {
                throw;
            }

            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }

        [Authorize]
        public async Task<IActionResult> OrderConformation(int Id)
        {
            if (Id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.GetUserAsync(User);
            var applicationUserId = user.Id;

            ViewBag.FirstName = user.FirstName;

            OrderHeader orderHeader = await _unitOfWork.CustomerRepo.GetFirstOrDefaultOrderHeaderAsync(x => x.Id == Id && x.ApplicationUserId == applicationUserId);

            if (orderHeader == null)
            {
                return RedirectToAction(nameof(Index));
            }

            if (orderHeader.PaymentStatus != StaticDetails.PaymentStatusPaid)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.CustomerRepo.UpdateStripeSessionToPaid(orderHeader.Id, session.PaymentIntentId, StaticDetails.PaymentStatusPaid, StaticDetails.StatusApproved);

                    try
                    {
                        _unitOfWork.Save();
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    HttpContext.Session.Clear();

                    return View();
                }
                else
                {
                    return RedirectToAction(nameof(Cart));
                }
            }
            else
            {
                return RedirectToAction(nameof(Cart));
            }
        }


        public IActionResult StoreLocator()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> OrderHistory()
        {
            var user = await _userManager.GetUserAsync(User);

            if (User.IsInRole(ApplicationRoles.App_User_Role_Test))
            {
                return View(await _unitOfWork.CustomerRepo.GetOrderHistory(user.Id, user.FirstName, true));
            }
            return View(await _unitOfWork.CustomerRepo.GetOrderHistory(user.Id, user.FirstName, false));
        }

    }
}
