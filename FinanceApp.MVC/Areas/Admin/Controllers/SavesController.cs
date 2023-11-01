using FinanceApp.BL.Concrete;
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
    public class SavesController : Controller
    {
       
        private readonly SqlDbContext dbContext;
        private readonly UserManager<AppUser> userManager;
        public SavesController(SqlDbContext dbContext, UserManager<AppUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            
        }
        public IActionResult Index()
        {
            var user = userManager.GetUserId(this.User);
            var result = dbContext.Saves.Where(x => x.AppUserId == user).ToList();
            return View(result);
            
        }
    }
}
