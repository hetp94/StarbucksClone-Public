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
    public class CustomizationNewsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomizationNewsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: CustomizationNews
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.AdminRepo.GetAllCustomizationListAsync());
        }

        // GET: CustomizationNews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customizationNew = await _unitOfWork.AdminRepo.GetFirstOrDefaultCustomizationAsync(m => m.CustomizationId == id);

            if (customizationNew == null)
            {
                return NotFound();
            }

            return View(customizationNew);
        }

        // GET: CustomizationNews/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CustomizationSubcategoryId"] = new SelectList(
                await _unitOfWork.AdminRepo.GetAllCustomizationSubCategoryListAsync(),
                nameof(CustomizationSubCategory.CustomizationSubcategoryId),
                nameof(CustomizationSubCategory.SubCategoryName));
            return View();
        }

        // POST: CustomizationNews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomizationId,CustomizationName,CustomizationSubcategoryId")] Customization customizationNew)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.AdminRepo.AddCustomization(customizationNew);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomizationSubcategoryId"] = new SelectList(
                await _unitOfWork.AdminRepo.GetAllCustomizationSubCategoryListAsync(),
                nameof(CustomizationSubCategory.CustomizationSubcategoryId),
                nameof(CustomizationSubCategory.SubCategoryName), customizationNew.CustomizationSubcategoryId);
            return View(customizationNew);
        }

        // GET: CustomizationNews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customizationNew = await _unitOfWork.AdminRepo.GetFirstOrDefaultCustomizationAsync(m => m.CustomizationId == id);
            if (customizationNew == null)
            {
                return NotFound();
            }
            ViewData["CustomizationSubcategoryId"] = new SelectList(
                           await _unitOfWork.AdminRepo.GetAllCustomizationSubCategoryListAsync(),
                           nameof(CustomizationSubCategory.CustomizationSubcategoryId),
                           nameof(CustomizationSubCategory.SubCategoryName), customizationNew.CustomizationSubcategoryId);

            return View(customizationNew);
        }

        // POST: CustomizationNews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomizationId,CustomizationName,CustomizationSubcategoryId")] Customization customizationNew)
        {
            if (id != customizationNew.CustomizationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.AdminRepo.UpdateCustomization(customizationNew);
                    await _unitOfWork.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!CustomizationNewExists(customizationNew.CustomizationId))

                    if (!await _unitOfWork.AdminRepo.IfCustomizationExist(x => x.CustomizationId == id))
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
            ViewData["CustomizationSubcategoryId"] = new SelectList(
                                      await _unitOfWork.AdminRepo.GetAllCustomizationSubCategoryListAsync(),
                                      nameof(CustomizationSubCategory.CustomizationSubcategoryId),
                                      nameof(CustomizationSubCategory.SubCategoryName), customizationNew.CustomizationSubcategoryId);
            return View(customizationNew);
        }

        // GET: CustomizationNews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customizationNew = await _unitOfWork.AdminRepo.GetFirstOrDefaultCustomizationAsync(m => m.CustomizationId == id);

            if (customizationNew == null)
            {
                return NotFound();
            }

            return View(customizationNew);
        }

        // POST: CustomizationNews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customizationNew = await _unitOfWork.AdminRepo.GetFirstOrDefaultCustomizationAsync(m => m.CustomizationId == id);
            if (customizationNew != null)
            {
                _unitOfWork.AdminRepo.RemoveCustomization(customizationNew);
            }

            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
