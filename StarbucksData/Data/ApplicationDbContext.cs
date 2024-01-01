using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StarbucksModels.DbModels;
namespace StarbucksWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //Menu Layout
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }

        public DbSet<Product> Product { get; set; }
        public DbSet<ProductCustomization> ProductCustomization { get; set; }
        public DbSet<CustomizationVisibility> CustomizationVisibility { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }



        //Customer

        public DbSet<Order> Order { get; set; }
        public DbSet<Cart> Cart { get; set; }

        public DbSet<OrderHeader> OrderHeader { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }



        //Customization 

        public DbSet<CustomizationCategory> CustomizationCategory { get; set; }

        public DbSet<CustomizationSubCategory> CustomizationSubCategory { get; set; }

        public DbSet<Customization> Customization { get; set; }

        public DbSet<CustomizationOption> CustomizationOption { get; set; }

        public DbSet<CustomizationModel> Customization1 { get; set; }

        public DbSet<SizeType> SizeType { get; set; }

        public DbSet<ProdCustVisibility> ProdCustVisibility { get; set; }

        public DbSet<ProdCustVisibilityOptions> ProdCustVisibilityOptions { get; set; }

        public DbSet<ProductSizeType> ProductSizeType { get; set; }

       
    }
}
