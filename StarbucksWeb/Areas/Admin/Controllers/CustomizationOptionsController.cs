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
    public class CustomizationOptionsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomizationOptionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: CustomizationOptions
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.AdminRepo.GetAllCustomizationOptionListAsync());
        }

        // GET: CustomizationOptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customizationOption = await _unitOfWork.AdminRepo.GetFirstOrDefaultCustomizationOptionAsync(m => m.CustomizationOptionId == id);
            if (customizationOption == null)
            {
                return NotFound();
            }

            return View(customizationOption);
        }

        // GET: CustomizationOptions/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CustomizationId"] = new SelectList(await _unitOfWork.AdminRepo.GetAllCustomizationListAsync(),
                 nameof(Customization.CustomizationId),
                 nameof(Customization.CustomizationName));
            return View();
        }

        // POST: CustomizationOptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomizationOptionId,CustomizationId,CustomizationOptionName")] CustomizationOption customizationOption)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.AdminRepo.AddCustomizationOption(customizationOption);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomizationId"] = new SelectList(await _unitOfWork.AdminRepo.GetAllCustomizationListAsync(),
                 nameof(Customization.CustomizationId),
                 nameof(Customization.CustomizationName), customizationOption.CustomizationId);
            return View(customizationOption);
        }

        // GET: CustomizationOptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customizationOption = await _unitOfWork.AdminRepo.GetFirstOrDefaultCustomizationOptionAsync(m => m.CustomizationOptionId == id);
            if (customizationOption == null)
            {
                return NotFound();
            }
            ViewData["CustomizationId"] = new SelectList(await _unitOfWork.AdminRepo.GetAllCustomizationListAsync(),
                 nameof(Customization.CustomizationId),
                 nameof(Customization.CustomizationName), customizationOption.CustomizationId);
            return View(customizationOption);
        }

        // POST: CustomizationOptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomizationOptionId,CustomizationId,CustomizationOptionName")] CustomizationOption customizationOption)
        {
            if (id != customizationOption.CustomizationOptionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.AdminRepo.UpdateCustomizationOption(customizationOption);
                    await _unitOfWork.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _unitOfWork.AdminRepo.IfCustomizationOptionExist(x => x.CustomizationOptionId == id))
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
            ViewData["CustomizationId"] = new SelectList(await _unitOfWork.AdminRepo.GetAllCustomizationListAsync(),
                 nameof(Customization.CustomizationId),
                 nameof(Customization.CustomizationName), customizationOption.CustomizationId);
            return View(customizationOption);
        }

        // GET: CustomizationOptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customizationOption = await _unitOfWork.AdminRepo.GetFirstOrDefaultCustomizationOptionAsync(m => m.CustomizationOptionId == id);

            if (customizationOption == null)
            {
                return NotFound();
            }

            return View(customizationOption);
        }

        // POST: CustomizationOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customizationOption = await _unitOfWork.AdminRepo.GetFirstOrDefaultCustomizationOptionAsync(m => m.CustomizationOptionId == id);
            if (customizationOption != null)
            {
                _unitOfWork.AdminRepo.RemoveCustomizationOption(customizationOption);
            }

            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
