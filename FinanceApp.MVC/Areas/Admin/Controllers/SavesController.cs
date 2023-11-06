using FinanceApp.BL.Concrete;
using FinanceApp.DAL.Context;
using FinanceApp.Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Save save = new Save();
            save = dbContext.Saves.FirstOrDefault(x => x.Id == id);
            var user = dbContext.Users.FirstOrDefault(x => x.Id == save.AppUserId);
            var goal = dbContext.Goals.FirstOrDefault(x=>x.AppUserId==user.Id);
           


            if (user != null)
            {
                if (save.Type == Tip.Giris)
                {
                    goal.TargetGoal -= save.Amount;
                    
                }
                else if (save.Type == Tip.Cikis)
                {
                    goal.TargetGoal += save.Amount;
                }



                dbContext.Goals.Update(goal);
                dbContext.Saves.Remove(save);
                dbContext.SaveChanges();

                return RedirectToAction("index");
            }

            return RedirectToAction("index");
        }
    }
}
