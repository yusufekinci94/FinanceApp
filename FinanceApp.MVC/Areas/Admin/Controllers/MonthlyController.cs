using FinanceApp.DAL.Context;
using FinanceApp.Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FinanceApp.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MonthlyController : Controller
    {
        private readonly SqlDbContext dbContext;
        private readonly UserManager<AppUser> userManager;


        public MonthlyController(SqlDbContext dbContext, UserManager<AppUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            var user = userManager.GetUserId(this.User);
            var result = dbContext.Monthlies.Where(x => x.AppUserId == user).ToList();
            return View(result);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var monthly = dbContext.Monthlies.Find(id);
            dbContext.Monthlies.Remove(monthly);
            dbContext.SaveChanges();
            return RedirectToAction("index");
        }

        
    }
}
