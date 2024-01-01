
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StarbucksData.IRepository;
using StarbucksModels.DbModels;
using StarbucksStaticDetails;
using StarbucksWeb.Models;


namespace StarbucksWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = ApplicationRoles.App_Admin_Role)]
    [Area(nameof(Admin))]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.AdminRepo.GetAllCategoryListAsync());
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _unitOfWork.AdminRepo.GetFirstOrDefaultCategoryAsync(x => x.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        public async Task<IActionResult> Create()
        {
            ViewData["MenuId"] = new SelectList(await _unitOfWork.AdminRepo.GetAllMenuListAsync(), "MenuId", "MenuName");
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,MenuId,CategoryName,ShowVisibility,SortingNumber,DateCreated,CreatedBy,DateUpdated,UpdatedBy,EfefctiveDate")] Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.AdminRepo.AddCategory(category);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuId"] = new SelectList(await _unitOfWork.AdminRepo.GetAllMenuListAsync(), "MenuId", "MenuName");
            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _unitOfWork.AdminRepo.GetFirstOrDefaultCategoryAsync(x => x.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["MenuId"] = new SelectList(await _unitOfWork.AdminRepo.GetAllMenuListAsync(), "MenuId", "MenuName");
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,MenuId,CategoryName,ShowVisibility,SortingNumber,DateCreated,CreatedBy,DateUpdated,UpdatedBy,EfefctiveDate")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.AdminRepo.UpdateCategory(category);
                    await _unitOfWork.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _unitOfWork.AdminRepo.IfCategoryExist(x => x.CategoryId == id))
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
            ViewData["MenuId"] = new SelectList(await _unitOfWork.AdminRepo.GetAllMenuListAsync(), "MenuId", "MenuName");
            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _unitOfWork.AdminRepo.GetFirstOrDefaultCategoryAsync(x => x.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _unitOfWork.AdminRepo.GetFirstOrDefaultCategoryAsync(x => x.CategoryId == id);
            if (category != null)
            {
                _unitOfWork.AdminRepo.RemoveCategory(category);
            }

            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
