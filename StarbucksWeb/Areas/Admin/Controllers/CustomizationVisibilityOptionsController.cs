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
    [Area(nameof(Admin))]
    [Authorize(Roles = ApplicationRoles.App_Admin_Role)]
    public class CustomizationVisibilityOptionsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomizationVisibilityOptionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: CustomizationVisibilityOptions
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.AdminRepo.GetAllCustomizationSubCategoryListAsync());
        }

        // GET: CustomizationVisibilityOptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customizationVisibilityOptions = await _unitOfWork.AdminRepo.GetFirstOrDefaultCustomizationSubCategoryAsync(x => x.CustomizationSubcategoryId == id);
            if (customizationVisibilityOptions == null)
            {
                return NotFound();
            }

            return View(customizationVisibilityOptions);
        }

        // GET: CustomizationVisibilityOptions/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CustomizationVisibilityCodeId"] = new SelectList(await _unitOfWork.AdminRepo.GetAllCustomizationCategoryListAsync()
                , nameof(CustomizationCategory.CustomizationCategoryId), nameof(CustomizationCategory.CategoryName));
            return View();
        }

        // POST: CustomizationVisibilityOptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OptionName,CustomizationVisibilityCodeId")] CustomizationSubCategory customizationVisibilityOptions)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.AdminRepo.AddCustomizationSubCategory(customizationVisibilityOptions);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomizationVisibilityCodeId"] = new SelectList(await _unitOfWork.AdminRepo.GetAllCustomizationCategoryListAsync()
               , nameof(CustomizationCategory.CustomizationCategoryId),
               nameof(CustomizationCategory.CategoryName),
               customizationVisibilityOptions.CustomizationCategoryId);
            return View(customizationVisibilityOptions);
        }

        // GET: CustomizationVisibilityOptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customizationVisibilityOptions = await _unitOfWork.AdminRepo.GetFirstOrDefaultCustomizationSubCategoryAsync(x => x.CustomizationSubcategoryId == id);
            if (customizationVisibilityOptions == null)
            {
                return NotFound();
            }
            ViewData["CustomizationVisibilityCodeId"] = new SelectList(await _unitOfWork.AdminRepo.GetAllCustomizationCategoryListAsync()
                , nameof(CustomizationCategory.CustomizationCategoryId),
                nameof(CustomizationCategory.CategoryName),
                customizationVisibilityOptions.CustomizationCategoryId);
            return View(customizationVisibilityOptions);
        }

        // POST: CustomizationVisibilityOptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OptionName,CustomizationVisibilityCodeId")] CustomizationSubCategory customizationVisibilityOptions)
        {
            if (id != customizationVisibilityOptions.CustomizationSubcategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.AdminRepo.UpdateCustomizationSubCategory(customizationVisibilityOptions);
                    await _unitOfWork.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _unitOfWork.AdminRepo.IfCustomizationSubCategoryExist(x => x.CustomizationSubcategoryId == id))
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
            ViewData["CustomizationVisibilityCodeId"] = new SelectList(await _unitOfWork.AdminRepo.GetAllCustomizationCategoryListAsync()
                , nameof(CustomizationCategory.CustomizationCategoryId),
                nameof(CustomizationCategory.CategoryName),
                customizationVisibilityOptions.CustomizationCategoryId);
            return View(customizationVisibilityOptions);
        }

        // GET: CustomizationVisibilityOptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customizationVisibilityOptions = await _unitOfWork.AdminRepo.GetFirstOrDefaultCustomizationSubCategoryAsync(x => x.CustomizationSubcategoryId == id);

            if (customizationVisibilityOptions == null)
            {
                return NotFound();
            }

            return View(customizationVisibilityOptions);
        }

        // POST: CustomizationVisibilityOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customizationVisibilityOptions = await _unitOfWork.AdminRepo.GetFirstOrDefaultCustomizationSubCategoryAsync(x => x.CustomizationSubcategoryId == id);

            if (customizationVisibilityOptions != null)
            {
                _unitOfWork.AdminRepo.RemoveCustomizationSubCategory(customizationVisibilityOptions);
            }

            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
