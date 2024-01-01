using StarbucksModels;
using StarbucksModels.DbModels;
using StarbucksModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StarbucksData.IRepository
{
    public interface IAdminRepository : IRepository<DummyClass>
    {
        //Menu
        Task<IEnumerable<Menu>> GetAllMenuListAsync();

        IEnumerable<Menu> GetAllMenuList();

        Task<Menu> GetFirstOrDefaultMenuAsync(Expression<Func<Menu, bool>> filter);

        void AddMenu(Menu menu);

        void UpdateMenu(Menu menu);

        void RemoveMenu(Menu menu);

        Task<bool> IfMenuExist(Expression<Func<Menu, bool>> filter);

        //Category
        Task<IEnumerable<Category>> GetAllCategoryListAsync(Expression<Func<Category, bool>>? filter = null);

        Task<Category> GetFirstOrDefaultCategoryAsync(Expression<Func<Category, bool>> filter);

        void AddCategory(Category category);

        void UpdateCategory(Category category);

        void RemoveCategory(Category category);

        Task<bool> IfCategoryExist(Expression<Func<Category, bool>> filter);

        //SubCategory
        Task<IEnumerable<SubCategory>> GetAllSubCategoryListAsync(Expression<Func<SubCategory, bool>>? filter = null);

        Task<SubCategory> GetFirstOrDefaultSubCategoryAsync(Expression<Func<SubCategory, bool>> filter);

        void AddSubCategory(SubCategory subcategory);

        void UpdateSubCategory(SubCategory subcategory);

        void RemoveSubCategory(SubCategory subcategory);

        Task<bool> IfSubCategoryExist(Expression<Func<SubCategory, bool>> filter);


        //Customization Category
        Task<IEnumerable<CustomizationCategory>> GetAllCustomizationCategoryListAsync();

        Task<CustomizationCategory> GetFirstOrDefaultCustomizationCategoryAsync(Expression<Func<CustomizationCategory, bool>> filter);

        void AddCustomizationCategory(CustomizationCategory obj);

        void UpdateCustomizationCategory(CustomizationCategory obj);

        void RemoveCustomizationCategory(CustomizationCategory obj);

        Task<bool> IfCustomizationCategoryExist(Expression<Func<CustomizationCategory, bool>> filter);

        //Customization SubCategory
        Task<IEnumerable<CustomizationSubCategory>> GetAllCustomizationSubCategoryListAsync();

        Task<CustomizationSubCategory> GetFirstOrDefaultCustomizationSubCategoryAsync(Expression<Func<CustomizationSubCategory, bool>> filter);

        void AddCustomizationSubCategory(CustomizationSubCategory obj);

        void UpdateCustomizationSubCategory(CustomizationSubCategory obj);

        void RemoveCustomizationSubCategory(CustomizationSubCategory obj);

        Task<bool> IfCustomizationSubCategoryExist(Expression<Func<CustomizationSubCategory, bool>> filter);


        //Customization 
        Task<IEnumerable<CustomizationNewVM>> GetAllCustomizationListAsync();

        Task<Customization> GetFirstOrDefaultCustomizationAsync(Expression<Func<Customization, bool>> filter);

        void AddCustomization(Customization obj);

        void UpdateCustomization(Customization obj);

        void RemoveCustomization(Customization obj);

        Task<bool> IfCustomizationExist(Expression<Func<Customization, bool>> filter);


        //Customization Options
        Task<IEnumerable<CustomizationOptionVM>> GetAllCustomizationOptionListAsync();

        Task<CustomizationOption> GetFirstOrDefaultCustomizationOptionAsync(Expression<Func<CustomizationOption, bool>> filter);

        void AddCustomizationOption(CustomizationOption obj);

        void UpdateCustomizationOption(CustomizationOption obj);

        void RemoveCustomizationOption(CustomizationOption obj);

        Task<bool> IfCustomizationOptionExist(Expression<Func<CustomizationOption, bool>> filter);

        //Product
        Task<IEnumerable<Product>> GetAllProductListAsync();

        Task<Product> GetFirstOrDefaultProductAsync(Expression<Func<Product, bool>> filter);

        void AddProduct(Product obj);

        void UpdateProduct(Product obj);

        void RemoveProduct(Product obj);

        Task<bool> IfProductExist(Expression<Func<Product, bool>> filter);



    }
}
