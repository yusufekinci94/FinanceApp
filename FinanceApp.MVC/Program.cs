using FinanceApp.DAL.Context;
using FinanceApp.Entities.Concrete;
using FinanceApp.MVC.CustomValidations;
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

            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedEmail = false; // Buayi Degistimeyi unutma

                options.Lockout.MaxFailedAccessAttempts = 5;


            }).AddPasswordValidator<CustomPasswordValidation>().AddErrorDescriber<CustomIdentityErrorDescriber>().AddUserValidator<CustomUserValidation>().AddEntityFrameworkStores<SqlDbContext>(); ;

            builder.Services.ConfigureApplicationCookie(c =>
            {
                c.LoginPath = new PathString("/Login/Login"); //// ASKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK
                c.Cookie = new CookieBuilder
                {
                    Name = "AspNetCoreIdentityExampleCookie",
                    HttpOnly = false,
                    SameSite = SameSiteMode.Lax,
                    SecurePolicy = CookieSecurePolicy.Always
                };
                c.SlidingExpiration = true;
                c.ExpireTimeSpan = TimeSpan.FromMinutes(2);
            });

            var app = builder.Build();

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}