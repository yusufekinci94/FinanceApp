using FinanceApp.DAL.Context;
using FinanceApp.Entities.Concrete;
using FinanceApp.MVC.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FinanceApp.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProgressController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SqlDbContext dbContext;
        public ProgressController(UserManager<AppUser> userManager, SqlDbContext dbContext)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
        }
        public IActionResult Index(ProgressDTO dTO)
        {
            var userid = userManager.GetUserId(this.User);
            dTO.Entries = dbContext.Entries.Where(x => x.AppUserId == userid).OrderBy(x=>x.CategoryIds).ToList();
            dTO.User = dbContext.Users.Find(userid);
            dTO.Monthlies = dbContext.Monthlies.Where(x => x.AppUserId == userid).OrderBy(x => x.Categories).ToList();
            dTO.Saves = dbContext.Saves.Where(x => x.AppUserId == userid).ToList();
            dTO.categories = dbContext.Entries.Where(x => x.AppUserId == userid).Select(a => a.CategoryIds).Distinct().ToList();
            dTO.Goal=dbContext.Goals.Where(x => x.AppUserId == userid).FirstOrDefault();
            var entries = dTO.Entries.Where(entry => entry.Amount < 200 && entry.Type == Tip.Cikis && !string.IsNullOrEmpty(entry.CategoryIds)).ToList();
            
            var repeatingOutgoings = entries.GroupBy(entry => entry.CategoryIds).Where(group => group.Count() > 1).SelectMany(group => group).ToList();

            dTO.RepeatingOutgoings = repeatingOutgoings;
            return View(dTO);
        }
    }
}
