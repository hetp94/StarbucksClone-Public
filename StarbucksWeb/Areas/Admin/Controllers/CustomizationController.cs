using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StarbucksModels.DbModels;
using StarbucksStaticDetails;
using StarbucksWeb.Data;
using StarbucksWeb.Models;

namespace StarbucksWeb.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = ApplicationRoles.App_Admin_Role)]
    public class CustomizationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomizationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customization
        public async Task<IActionResult> Index()
        {
            UIDetail.ActiveController = "Customization";
            return View(await _context.Customization1.OrderBy(x => x.CustomizationType).ThenBy(y => y.SubCustomizationType).ThenBy(x => x.Customization).ThenBy(x => x.SubCustomization).ToListAsync());
        }

        // GET: Customization/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customization1 == null)
            {
                return NotFound();
            }

            var customizationModel = await _context.Customization1
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customizationModel == null)
            {
                return NotFound();
            }

            return View(customizationModel);
        }

        #region Customization Type

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomizationType,SubCustomizationType,Customization,SubCustomization,Qty")] CustomizationModel customizationModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customizationModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customizationModel);
        }

        // GET: Customization/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customization1 == null)
            {
                return NotFound();
            }

            var customizationModel = await _context.Customization1.FindAsync(id);
            if (customizationModel == null)
            {
                return NotFound();
            }
            return View(customizationModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomizationType,SubCustomizationType,Customization,SubCustomization,Qty")] CustomizationModel customizationModel)
        {
            if (id != customizationModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    CustomizationModel customizationModelFromDb = _context.Customization1.FirstOrDefault(x => x.Id == id);

                    List<CustomizationModel> customizationModelFromDbRange = _context.Customization1.Where(x => x.CustomizationType == customizationModelFromDb.CustomizationType).ToList();

                    if (customizationModelFromDbRange != null)
                    {
                        foreach (CustomizationModel item in customizationModelFromDbRange)
                        {
                            item.CustomizationType = customizationModel.CustomizationType;
                        }
                    }
                    _context.UpdateRange(customizationModelFromDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomizationModelExists(customizationModel.Id))
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
            return View(customizationModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customization1 == null)
            {
                return NotFound();
            }

            var customizationModel = await _context.Customization1
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customizationModel == null)
            {
                return NotFound();
            }

            return View(customizationModel);
        }

        // POST: Customization/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customization1 == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Customization'  is null.");
            }
            var customizationModel = await _context.Customization1.FindAsync(id);

            List<CustomizationModel> customizationModelFromDbRange = _context.Customization1.Where(x => x.CustomizationType == customizationModel.CustomizationType).ToList();

            if (customizationModelFromDbRange != null)
            {
                _context.Customization1.RemoveRange(customizationModelFromDbRange);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        #endregion





        //Sub Customization Type
        public IActionResult CreateSubCustomizationType()
        {
            CustomizationModel customizationAttribute = new CustomizationModel();
            customizationAttribute.CustomizationTypeDropDown = GetCustomizationTypeList();
            return View(customizationAttribute);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateSubCustomizationType(CustomizationModel customizationFromView)
        {
            CustomizationModel customizationAttributeFromDb = new CustomizationModel();
            if (ModelState.IsValid)
            {
                customizationAttributeFromDb = _context.Customization1.FirstOrDefault(m => m.CustomizationType == customizationFromView.CustomizationType);
                if (customizationAttributeFromDb == null)
                {
                    return NotFound();
                }
                try
                {
                    if (customizationAttributeFromDb.SubCustomizationType == null)
                    {
                        customizationAttributeFromDb.SubCustomizationType = customizationFromView.SubCustomizationType;
                        _context.Customization1.Update(customizationAttributeFromDb);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _context.Customization1.Add(customizationFromView);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbException ex)
                {
                    Console.WriteLine("Database Error: " + ex.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    throw;
                }
            }
            customizationFromView.CustomizationTypeDropDown = GetCustomizationTypeList();
            return View(customizationFromView);
        }


        public async Task<IActionResult> EditSubCustomizationType(int? id)
        {
            if (id == null || _context.Customization1 == null)
            {
                return NotFound();
            }

            var customizationModel = await _context.Customization1.FindAsync(id);
            if (customizationModel == null)
            {
                return NotFound();
            }
            return View(customizationModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubCustomizationType(int id, [Bind("Id,CustomizationType,SubCustomizationType,Customization,SubCustomization,Qty")] CustomizationModel customizationModel)
        {
            if (id != customizationModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    CustomizationModel customizationModelFromDb = _context.Customization1.FirstOrDefault(x => x.Id == id);

                    List<CustomizationModel> customizationModelFromDbRange = _context.Customization1.Where(x => x.SubCustomizationType == customizationModelFromDb.SubCustomizationType).ToList();

                    if (customizationModelFromDbRange != null)
                    {
                        foreach (CustomizationModel item in customizationModelFromDbRange)
                        {
                            item.SubCustomizationType = customizationModel.SubCustomizationType;
                        }
                    }
                    _context.UpdateRange(customizationModelFromDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomizationModelExists(customizationModel.Id))
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
            return View(customizationModel);
        }

        public async Task<IActionResult> DeleteSubCustomizationType(int? id)
        {
            if (id == null || _context.Customization1 == null)
            {
                return NotFound();
            }

            var customizationModel = await _context.Customization1
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customizationModel == null)
            {
                return NotFound();
            }

            return View(customizationModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubCustomizationType(int id)
        {
            if (_context.Customization1 == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Customization'  is null.");
            }
            var customizationModel = await _context.Customization1.FindAsync(id);

            List<CustomizationModel> customizationModelFromDbRange = _context.Customization1.Where(x => x.SubCustomizationType == customizationModel.SubCustomizationType).ToList();

            if (customizationModelFromDbRange != null)
            {
                _context.Customization1.RemoveRange(customizationModelFromDbRange);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }






        //Customization
        public IActionResult CreateCustomization()
        {
            CustomizationModel customizationAttribute = new CustomizationModel();
            customizationAttribute.CustomizationTypeDropDown = GetCustomizationTypeList();
            return View(customizationAttribute);
        }
        [HttpGet]
        public JsonResult GetSubCustomizationType(string customizationTypeId)
        {
            IEnumerable<SelectListItem> subCustomizationTypeId = new List<SelectListItem>();
            subCustomizationTypeId = GetSubCustomizationTypeList(customizationTypeId);
            return Json(subCustomizationTypeId);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCustomization(CustomizationModel customizationFromView)
        {
            CustomizationModel customizationAttributeFromDb = new CustomizationModel();
            if (ModelState.IsValid)
            {
                customizationAttributeFromDb = _context.Customization1.FirstOrDefault(m => m.SubCustomizationType == customizationFromView.SubCustomizationType);
                if (customizationAttributeFromDb == null)
                {
                    return NotFound();
                }
                try
                {
                    if (customizationAttributeFromDb.Customization == null)
                    {
                        customizationAttributeFromDb.Customization = customizationFromView.Customization;
                        _context.Customization1.Update(customizationAttributeFromDb);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _context.Customization1.Add(customizationFromView);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbException ex)
                {
                    Console.WriteLine("Database Error: " + ex.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    throw;
                }
            }
            customizationFromView.CustomizationTypeDropDown = GetCustomizationTypeList();
            return View(customizationFromView);
        }

        public async Task<IActionResult> EditCustomization(int? id)
        {
            if (id == null || _context.Customization1 == null)
            {
                return NotFound();
            }

            var customizationModel = await _context.Customization1.FindAsync(id);
            if (customizationModel == null)
            {
                return NotFound();
            }
            return View(customizationModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomization(int id, [Bind("Id,CustomizationType,SubCustomizationType,Customization,SubCustomization,Qty")] CustomizationModel customizationModel)
        {
            if (id != customizationModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    CustomizationModel customizationModelFromDb = _context.Customization1.FirstOrDefault(x => x.Id == id);

                    List<CustomizationModel> customizationModelFromDbRange = _context.Customization1.Where(x => x.Customization == customizationModelFromDb.Customization).ToList();

                    if (customizationModelFromDbRange != null)
                    {
                        foreach (CustomizationModel item in customizationModelFromDbRange)
                        {
                            item.Customization = customizationModel.Customization;
                        }
                    }
                    _context.UpdateRange(customizationModelFromDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomizationModelExists(customizationModel.Id))
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
            return View(customizationModel);
        }

        public async Task<IActionResult> DeleteCustomization(int? id)
        {
            if (id == null || _context.Customization1 == null)
            {
                return NotFound();
            }

            var customizationModel = await _context.Customization1
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customizationModel == null)
            {
                return NotFound();
            }

            return View(customizationModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCustomization(int id)
        {
            if (_context.Customization1 == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Customization'  is null.");
            }
            var customizationModel = await _context.Customization1.FindAsync(id);

            List<CustomizationModel> customizationModelFromDbRange = _context.Customization1.Where(x => x.Customization == customizationModel.Customization).ToList();

            if (customizationModelFromDbRange != null)
            {
                _context.Customization1.RemoveRange(customizationModelFromDbRange);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        //Sub Customization
        public IActionResult CreateSubCustomization()
        {
            CustomizationModel customizationAttribute = new CustomizationModel();
            customizationAttribute.CustomizationTypeDropDown = GetCustomizationTypeList();
            return View(customizationAttribute);
        }
        [HttpGet]
        public JsonResult GetCustomization(string subCustomizationTypeId)
        {
            IEnumerable<SelectListItem> customizationList = new List<SelectListItem>();
            customizationList = GetCustomizationList(subCustomizationTypeId);
            return Json(customizationList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateSubCustomization(CustomizationModel customizationFromView)
        {
            CustomizationModel customizationAttributeFromDb = new CustomizationModel();
            if (ModelState.IsValid)
            {
                customizationAttributeFromDb = _context.Customization1.FirstOrDefault(m => m.Customization == customizationFromView.Customization);
                if (customizationAttributeFromDb == null)
                {
                    return NotFound();
                }
                try
                {
                    if (customizationAttributeFromDb.SubCustomization == null)
                    {
                        customizationAttributeFromDb.SubCustomization = customizationFromView.SubCustomization;
                        _context.Customization1.Update(customizationAttributeFromDb);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _context.Customization1.Add(customizationFromView);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbException ex)
                {
                    Console.WriteLine("Database Error: " + ex.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    throw;
                }
            }
            customizationFromView.CustomizationTypeDropDown = GetCustomizationTypeList();
            return View(customizationFromView);
        }

        public async Task<IActionResult> EditSubCustomization(int? id)
        {
            if (id == null || _context.Customization1 == null)
            {
                return NotFound();
            }

            var customizationModel = await _context.Customization1.FindAsync(id);
            if (customizationModel == null)
            {
                return NotFound();
            }
            return View(customizationModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubCustomization(int id, [Bind("Id,CustomizationType,SubCustomizationType,Customization,SubCustomization,Qty")] CustomizationModel customizationModel)
        {
            if (id != customizationModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    CustomizationModel customizationModelFromDb = _context.Customization1.FirstOrDefault(x => x.Id == id);
                    customizationModelFromDb.SubCustomization = customizationModel.SubCustomization;
                    _context.Update(customizationModelFromDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomizationModelExists(customizationModel.Id))
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
            return View(customizationModel);
        }

        public async Task<IActionResult> DeleteSubCustomization(int? id)
        {
            if (id == null || _context.Customization1 == null)
            {
                return NotFound();
            }

            var customizationModel = await _context.Customization1
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customizationModel == null)
            {
                return NotFound();
            }

            return View(customizationModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubCustomization(int id)
        {
            if (_context.Customization1 == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Customization'  is null.");
            }
            var customizationModel = await _context.Customization1.FindAsync(id);
            if (customizationModel != null)
            {
                _context.Customization1.Remove(customizationModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        private bool CustomizationModelExists(int id)
        {
            return _context.Customization1.Any(e => e.Id == id);
        }


        private IEnumerable<SelectListItem> GetCustomizationTypeList()
        {
            //List<string> groupedCustomerList = _context.CustomizationAttribute.Select(x=>x.CustomizationType).Distinct().ToList();



            IEnumerable<SelectListItem> customizationTypeList = new List<SelectListItem>();

            //customizationTypeList = _context.CustomizationAttribute.ToList().Select(i => new SelectListItem
            //{

            //    Text = i.CustomizationType,
            //    Value = i.CustomizationType,
            //});

            customizationTypeList = _context.Customization1.Select(x => x.CustomizationType).Distinct().ToList().Select(i => new SelectListItem
            {

                Text = i,
                Value = i,
            });

            return customizationTypeList;
        }

        private IEnumerable<SelectListItem> GetSubCustomizationTypeList(string CustomizationType)
        {
            //List<string> groupedCustomerList = _context.CustomizationAttribute.Select(x=>x.CustomizationType).Distinct().ToList();



            IEnumerable<SelectListItem> customizationTypeList = new List<SelectListItem>();

            //customizationTypeList = _context.CustomizationAttribute.ToList().Select(i => new SelectListItem
            //{

            //    Text = i.CustomizationType,
            //    Value = i.CustomizationType,
            //});

            customizationTypeList = _context.Customization1.Where(y => y.CustomizationType == CustomizationType).Select(x => x.SubCustomizationType).Distinct().ToList().Select(i => new SelectListItem
            {

                Text = i,
                Value = i,
            });

            return customizationTypeList;
        }

        private IEnumerable<SelectListItem> GetCustomizationList(string SubCustomizationType)
        {
            //List<string> groupedCustomerList = _context.CustomizationAttribute.Select(x=>x.CustomizationType).Distinct().ToList();



            IEnumerable<SelectListItem> customizationTypeList = new List<SelectListItem>();

            //customizationTypeList = _context.CustomizationAttribute.ToList().Select(i => new SelectListItem
            //{

            //    Text = i.CustomizationType,
            //    Value = i.CustomizationType,
            //});

            customizationTypeList = _context.Customization1.Where(y => y.SubCustomizationType == SubCustomizationType).Select(x => x.Customization).Distinct().ToList().Select(i => new SelectListItem
            {
                Text = i,
                Value = i,
            });

            return customizationTypeList;
        }
    }
}
