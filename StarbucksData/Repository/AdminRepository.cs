using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StarbucksData.IRepository;
using StarbucksModels;
using StarbucksModels.DbModels;
using StarbucksModels.ViewModels;
using StarbucksWeb.Data;
using System.Linq.Expressions;


namespace StarbucksData.Repository
{
    public class AdminRepository : Repository<DummyClass>, IAdminRepository
    {
        private ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext db) : base(db)
        {
            _context = db;
        }

        public void AddCategory(Category category)
        {
            _context.Category.Add(category);
        }

        public void AddCustomization(Customization obj)
        {
            _context.Customization.Add(obj);
        }

        public void AddCustomizationCategory(CustomizationCategory obj)
        {
            _context.CustomizationCategory.Add(obj);
        }

        public void AddCustomizationOption(CustomizationOption obj)
        {
            _context.CustomizationOption.Add(obj);
        }

        public void AddCustomizationSubCategory(CustomizationSubCategory obj)
        {
            _context.CustomizationSubCategory.Add(obj);
        }

        public void AddMenu(Menu menu)
        {
            _context.Menu.Add(menu);
        }

        public void AddProduct(Product obj)
        {
            throw new NotImplementedException();
        }

        public void AddSubCategory(SubCategory subcategory)
        {
            _context.SubCategory.Add(subcategory);
        }

        public async Task<IEnumerable<Category>> GetAllCategoryListAsync(Expression<Func<Category, bool>>? filter = null)
        {
            if (filter == null)
            {
                return await _context.Category.Include(x => x.Menu).ToListAsync();
            }
            else
            {
                return await _context.Category.Where(filter).Include(x => x.Menu).ToListAsync();
            }
        }

        public async Task<IEnumerable<CustomizationCategory>> GetAllCustomizationCategoryListAsync()
        {
            return await _context.CustomizationCategory.ToListAsync();
        }

        public async Task<IEnumerable<CustomizationNewVM>> GetAllCustomizationListAsync()
        {
            var query = (from custCate in _context.Set<CustomizationCategory>()
                         from custSubCat in _context.Set<CustomizationSubCategory>()
                         .Where(x => x.CustomizationCategoryId == custCate.CustomizationCategoryId)
                         from custModel in _context.Set<Customization>()
                         .Where(x => x.CustomizationSubcategoryId == custSubCat.CustomizationSubcategoryId)
                         select new CustomizationNewVM
                         {
                             CustomizationId = custModel.CustomizationId,
                             CustomizationName = custModel.CustomizationName,
                             CustomizationCategoryName = custCate.CategoryName,
                             CustomizationSubCategoryName = custSubCat.SubCategoryName
                         })
                         .Distinct()
                         .OrderBy(x => x.CustomizationCategoryName)
                         .ThenBy(x => x.CustomizationSubCategoryName)
                         .ThenBy(x => x.CustomizationName);

            await Console.Out.WriteLineAsync("All Customization: " + query.ToQueryString());

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<CustomizationOptionVM>> GetAllCustomizationOptionListAsync()
        {
            var query = (from custCate in _context.Set<CustomizationCategory>()
                         from custSubCat in _context.Set<CustomizationSubCategory>().Where(x => x.CustomizationCategoryId == custCate.CustomizationCategoryId)
                         from custModel in _context.Set<Customization>().Where(x => x.CustomizationSubcategoryId == custSubCat.CustomizationSubcategoryId)
                         from custOptionModel in _context.Set<CustomizationOption>().Where(x => x.CustomizationId == custModel.CustomizationId)

                         select new CustomizationOptionVM
                         {
                             CustomizationId = custModel.CustomizationId,
                             CustomizationName = custModel.CustomizationName,
                             CustomizationCategoryName = custCate.CategoryName,
                             CustomizationSubCategoryName = custSubCat.SubCategoryName,
                             CustomizationOptionName = custOptionModel.CustomizationOptionName,
                         })
                         .Distinct()
                         .OrderBy(x => x.CustomizationCategoryName)
                         .ThenBy(x => x.CustomizationSubCategoryName)
                         .ThenBy(x => x.CustomizationName);

            Console.WriteLine("All Customization Options: " + query.ToQueryString());

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<CustomizationSubCategory>> GetAllCustomizationSubCategoryListAsync()
        {
            return await _context.CustomizationSubCategory.Include(c => c.CustomizationCategory).OrderBy(x => x.CustomizationCategory.CategoryName).ThenBy(x => x.SubCategoryName).ToListAsync();
        }

        public IEnumerable<Menu> GetAllMenuList()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Menu>> GetAllMenuListAsync()
        {
            return await _context.Menu.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductListAsync()
        {
            return await _context.Product.ToListAsync();
        }

        public async Task<IEnumerable<SubCategory>> GetAllSubCategoryListAsync(Expression<Func<SubCategory, bool>>? filter = null)
        {
            if(filter == null)
            {
                return await _context.SubCategory.Include(s => s.Category).OrderBy(x => x.Category.CategoryName).ThenBy(x => x.SubCategoryName).ToListAsync();
            }
            else
            {
                return await _context.SubCategory.Where(filter).Include(s => s.Category).OrderBy(x => x.Category.CategoryName).ThenBy(x => x.SubCategoryName).ToListAsync();
            }
        }

        public async Task<Category> GetFirstOrDefaultCategoryAsync(Expression<Func<Category, bool>> filter)
        {
            return await _context.Category.Where(filter).FirstOrDefaultAsync();
        }

        public async Task<Customization> GetFirstOrDefaultCustomizationAsync(Expression<Func<Customization, bool>> filter)
        {
            return await _context.Customization.Include(c => c.CustomizationSubCategory).Where(filter).FirstOrDefaultAsync();
        }

        public async Task<CustomizationCategory> GetFirstOrDefaultCustomizationCategoryAsync(Expression<Func<CustomizationCategory, bool>> filter)
        {
            return await _context.CustomizationCategory.Where(filter).FirstOrDefaultAsync();
        }

        public async Task<CustomizationOption> GetFirstOrDefaultCustomizationOptionAsync(Expression<Func<CustomizationOption, bool>> filter)
        {
            return await _context.CustomizationOption.Where(filter).FirstOrDefaultAsync();
        }

        public async Task<CustomizationSubCategory> GetFirstOrDefaultCustomizationSubCategoryAsync(Expression<Func<CustomizationSubCategory, bool>> filter)
        {
            return await _context.CustomizationSubCategory.Include(c => c.CustomizationCategory).Where(filter).FirstOrDefaultAsync();
        }

        public async Task<Menu> GetFirstOrDefaultMenuAsync(Expression<Func<Menu, bool>> filter)
        {
            return await _context.Menu.Where(filter).FirstOrDefaultAsync();
        }

        public async Task<Product> GetFirstOrDefaultProductAsync(Expression<Func<Product, bool>> filter)
        {
            return await _context.Product.Where(filter).FirstOrDefaultAsync();
        }

        public async Task<SubCategory> GetFirstOrDefaultSubCategoryAsync(Expression<Func<SubCategory, bool>> filter)
        {
            return await _context.SubCategory.Include(s => s.Category).Where(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> IfCategoryExist(Expression<Func<Category, bool>> filter)
        {
            return await _context.Category.AnyAsync(filter);
        }

        public async Task<bool> IfCustomizationCategoryExist(Expression<Func<CustomizationCategory, bool>> filter)
        {
            return await _context.CustomizationCategory.AnyAsync(filter);
        }

        public async Task<bool> IfCustomizationExist(Expression<Func<Customization, bool>> filter)
        {
            return await _context.Customization.AnyAsync(filter);
        }

        public Task<bool> IfCustomizationOptionExist(Expression<Func<CustomizationOption, bool>> filter)
        {
            return _context.CustomizationOption.AnyAsync(filter);
        }

        public async Task<bool> IfCustomizationSubCategoryExist(Expression<Func<CustomizationSubCategory, bool>> filter)
        {
            return await _context.CustomizationSubCategory.AnyAsync(filter);
        }

        public async Task<bool> IfMenuExist(Expression<Func<Menu, bool>> filter)
        {
            return await _context.Menu.AnyAsync(filter);
        }

        public Task<bool> IfProductExist(Expression<Func<Product, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IfSubCategoryExist(Expression<Func<SubCategory, bool>> filter)
        {
            return await _context.SubCategory.AnyAsync(filter);
        }

        public void RemoveCategory(Category category)
        {
            _context.Category.Remove(category);
        }

        public void RemoveCustomization(Customization obj)
        {
            _context.Customization.Remove(obj);
        }

        public void RemoveCustomizationCategory(CustomizationCategory obj)
        {
            _context.CustomizationCategory.Remove(obj);
        }

        public void RemoveCustomizationOption(CustomizationOption obj)
        {
            _context.CustomizationOption.Remove(obj);
        }

        public void RemoveCustomizationSubCategory(CustomizationSubCategory obj)
        {
            _context.CustomizationSubCategory.Remove(obj);
        }

        public void RemoveMenu(Menu menu)
        {
            _context.Menu.Remove(menu);
        }

        public void RemoveProduct(Product obj)
        {
            throw new NotImplementedException();
        }

        public void RemoveSubCategory(SubCategory subcategory)
        {
            _context.SubCategory.Remove(subcategory);
        }

        public void UpdateCategory(Category category)
        {
            _context.Category.Update(category);
        }

        public void UpdateCustomization(Customization obj)
        {
            _context.Customization.Update(obj);
        }

        public void UpdateCustomizationCategory(CustomizationCategory obj)
        {
            _context.CustomizationCategory.Update(obj);
        }

        public void UpdateCustomizationOption(CustomizationOption obj)
        {
            _context.CustomizationOption.Update(obj);
        }

        public void UpdateCustomizationSubCategory(CustomizationSubCategory obj)
        {
            _context.CustomizationSubCategory.Update(obj);
        }

        public void UpdateMenu(Menu menu)
        {
            _context.Menu.Update(menu);
        }

        public void UpdateProduct(Product obj)
        {
            throw new NotImplementedException();
        }

        public void UpdateSubCategory(SubCategory subcategory)
        {
            _context.SubCategory.Update(subcategory);
        }
    }
}
