using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using StarbucksStaticDetails;
using Stripe;
using StarbucksWeb.Data;
using StarbucksData.IRepository;
using StarbucksData.Repository;
using StarbucksModels.DbModels;

namespace StarbucksWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession();
            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(
               builder.Configuration.GetConnectionString("DefaultConnection")
               ));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddDefaultTokenProviders()
                            .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddSingleton<IEmailSender, EmailSender>();
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();

            //       app.MapControllerRoute(
            //name: "Admin",
            //pattern: "{area:exists}/{controller=Category}/{action=Index}/{id?}");
            app.MapControllerRoute(
    name: "Customer",
    pattern: "{area:exists}/{controller=Menu}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            //pattern: "{area:Admin}/{controller=Category}/{action=Index}/{id?}");



            app.Run();
        }
    }
}