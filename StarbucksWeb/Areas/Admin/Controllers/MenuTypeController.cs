using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class MenuTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public MenuTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: MenuType
        public async Task<IActionResult> Index()
        {
            UIDetail.ActiveController = "Menu";
           
            return View(await _unitOfWork.AdminRepo.GetAllMenuListAsync());
        }

        // GET: MenuType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _unitOfWork.AdminRepo.GetFirstOrDefaultMenuAsync(x => x.MenuId == id);

            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: MenuType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MenuType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MenuId,MenuName,ShowVisibility,SortingNumber,DateCreated,CreatedBy,DateUpdated,UpdatedBy,EffectiveDate")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.AdminRepo.AddMenu(menu);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(menu);
        }

        // GET: MenuType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _unitOfWork.AdminRepo.GetFirstOrDefaultMenuAsync(x => x.MenuId == id);

            if (menu == null)
            {
                return NotFound();
            }
            return View(menu);
        }

        // POST: MenuType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MenuId,MenuName,ShowVisibility,SortingNumber,DateCreated,CreatedBy,DateUpdated,UpdatedBy,EffectiveDate")] Menu menu)
        {
            if (id != menu.MenuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                     _unitOfWork.AdminRepo.UpdateMenu(menu);
                    await _unitOfWork.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _unitOfWork.AdminRepo.IfMenuExist(x => x.MenuId == id))
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
            return View(menu);
        }

        // GET: MenuType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _unitOfWork.AdminRepo.GetFirstOrDefaultMenuAsync(x => x.MenuId == id);

            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: MenuType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menu = await _unitOfWork.AdminRepo.GetFirstOrDefaultMenuAsync(x => x.MenuId == id);
            if (menu != null)
            {
                _unitOfWork.AdminRepo.RemoveMenu(menu);
            }

            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
