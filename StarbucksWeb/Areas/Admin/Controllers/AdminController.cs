using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarbucksStaticDetails;

namespace StarbucksWeb.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = ApplicationRoles.App_Admin_Role)]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
