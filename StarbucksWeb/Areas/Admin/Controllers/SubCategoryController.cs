
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StarbucksData.IRepository;
using StarbucksModels.DbModels;
using StarbucksStaticDetails;

namespace StarbucksWeb.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = ApplicationRoles.App_Admin_Role)]
    public class SubCategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubCategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: SubCategory
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.AdminRepo.GetAllSubCategoryListAsync());
        }

        // GET: SubCategory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _unitOfWork.AdminRepo.GetFirstOrDefaultSubCategoryAsync(x => x.SubcategoryId == id);

            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        // GET: SubCategory/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _unitOfWork.AdminRepo.GetAllCategoryListAsync(), "CategoryId", "CategoryName");
            return View();
        }

        // POST: SubCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubcategoryId,CategoryId,SubCategoryName,ShowItem,SortingNumber,DateCreated,CreatedBy,DateUpdated,UpdatedBy,EffectiveDate")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.AdminRepo.AddSubCategory(subCategory);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(await _unitOfWork.AdminRepo.GetAllCategoryListAsync(),
                nameof(Category.CategoryId), "CategoryName", subCategory.CategoryId);
            return View(subCategory);
        }

        // GET: SubCategory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _unitOfWork.AdminRepo.GetFirstOrDefaultSubCategoryAsync(x => x.SubcategoryId == id);
            if (subCategory == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(await _unitOfWork.AdminRepo.GetAllCategoryListAsync(),
                nameof(Category.CategoryId), "CategoryName", subCategory.CategoryId);
            return View(subCategory);
        }

        // POST: SubCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubcategoryId,CategoryId,SubCategoryName,ShowItem,SortingNumber,DateCreated,CreatedBy,DateUpdated,UpdatedBy,EffectiveDate")] SubCategory subCategory)
        {
            if (id != subCategory.SubcategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.AdminRepo.UpdateSubCategory(subCategory);
                    await _unitOfWork.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _unitOfWork.AdminRepo.IfSubCategoryExist(x => x.SubcategoryId == id))
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
            ViewData["CategoryId"] = new SelectList(await _unitOfWork.AdminRepo.GetAllCategoryListAsync(),
                nameof(Category.CategoryId), "CategoryName", subCategory.CategoryId);
            return View(subCategory);
        }

        // GET: SubCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _unitOfWork.AdminRepo.GetFirstOrDefaultSubCategoryAsync(x => x.SubcategoryId == id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        // POST: SubCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subCategory = await _unitOfWork.AdminRepo.GetFirstOrDefaultSubCategoryAsync(x => x.SubcategoryId == id);
            if (subCategory != null)
            {
                _unitOfWork.AdminRepo.RemoveSubCategory(subCategory);
            }

            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
