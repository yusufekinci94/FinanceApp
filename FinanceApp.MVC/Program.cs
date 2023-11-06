using FinanceApp.BL.Abstract;
using FinanceApp.BL.Concrete;
using FinanceApp.DAL.Context;
using FinanceApp.DAL.Repositories.Abstract;
using FinanceApp.DAL.Repositories.Concrete;
using FinanceApp.Entities.Concrete;
using FinanceApp.MVC.CustomValidations;
using FinanceApp.MVC.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<SqlDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FinansDb")));
            //builder.Services.AddDefaultIdentity<AppUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<SqlDbContext>();

            builder.Services.AddAutoMapper(typeof(MapsManager));
            builder.Services.AddScoped<ICategoryManager, CategoryManager>();
			builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
			builder.Services.AddScoped<IEntryManager, EntryManager>();
			builder.Services.AddScoped<IEntryRepository, EntryRepository>();
			builder.Services.AddScoped<CategoryService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<CategoryService>();
            builder.Services.AddSingleton<IDailyTask, DailyTask>();

           


            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedEmail = false; // Buayi Degistimeyi unutma
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;


            }).AddPasswordValidator<CustomPasswordValidation>().AddErrorDescriber<CustomIdentityErrorDescriber>().AddUserValidator<CustomUserValidation>().AddEntityFrameworkStores<SqlDbContext>(); ;
          
            builder.Services.ConfigureApplicationCookie(c =>
            {
                c.LoginPath = "/Login/Login"; //// ASKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK
                c.Cookie = new CookieBuilder
                {
                    Name = "FinansAppCookie",
                    HttpOnly = false,
                    SameSite = SameSiteMode.Lax,
                    SecurePolicy = CookieSecurePolicy.Always




                };
                c.AccessDeniedPath = "/Login/AcessDenied";
                c.LogoutPath = "/Login/Logout";
                c.SlidingExpiration = true;
                //c.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            });
            
            var app = builder.Build();
            var dailyTask = app.Services.GetRequiredService<IDailyTask>();
            dailyTask.Start();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}