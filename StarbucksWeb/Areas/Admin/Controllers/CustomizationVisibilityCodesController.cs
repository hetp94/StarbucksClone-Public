
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using StarbucksData.IRepository;
using StarbucksModels.DbModels;
using StarbucksStaticDetails;
using StarbucksWeb.Data;
using StarbucksWeb.Models;

namespace StarbucksWeb.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = ApplicationRoles.App_Admin_Role)]
    public class CustomizationVisibilityCodesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomizationVisibilityCodesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: CustomizationVisibilityCodes
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.AdminRepo.GetAllCustomizationCategoryListAsync());
        }

        // GET: CustomizationVisibilityCodes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customizationVisibilityCode = await _unitOfWork.AdminRepo.GetFirstOrDefaultCustomizationCategoryAsync(m => m.CustomizationCategoryId == id);
            if (customizationVisibilityCode == null)
            {
                return NotFound();
            }

            return View(customizationVisibilityCode);
        }

        // GET: CustomizationVisibilityCodes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CustomizationVisibilityCodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomizationName")] CustomizationCategory customizationVisibilityCode)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.AdminRepo.AddCustomizationCategory(customizationVisibilityCode);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customizationVisibilityCode);
        }

        // GET: CustomizationVisibilityCodes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customizationVisibilityCode = await _unitOfWork.AdminRepo.GetFirstOrDefaultCustomizationCategoryAsync(m => m.CustomizationCategoryId == id);
            if (customizationVisibilityCode == null)
            {
                return NotFound();
            }
            return View(customizationVisibilityCode);
        }

        // POST: CustomizationVisibilityCodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomizationName")] CustomizationCategory customizationVisibilityCode)
        {
            if (id != customizationVisibilityCode.CustomizationCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.AdminRepo.UpdateCustomizationCategory(customizationVisibilityCode);
                    await _unitOfWork.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _unitOfWork.AdminRepo.IfCustomizationCategoryExist(x => x.CustomizationCategoryId == id))
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
            return View(customizationVisibilityCode);
        }

        // GET: CustomizationVisibilityCodes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customizationVisibilityCode = await _unitOfWork.AdminRepo.GetFirstOrDefaultCustomizationCategoryAsync(m => m.CustomizationCategoryId == id);
            if (customizationVisibilityCode == null)
            {
                return NotFound();
            }

            return View(customizationVisibilityCode);
        }

        // POST: CustomizationVisibilityCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customizationVisibilityCode = await _unitOfWork.AdminRepo.GetFirstOrDefaultCustomizationCategoryAsync(m => m.CustomizationCategoryId == id);

            if (customizationVisibilityCode != null)
            {
                _unitOfWork.AdminRepo.RemoveCustomizationCategory(customizationVisibilityCode);
            }

            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
