using FinanceApp.DAL.Context;
using FinanceApp.Entities.Concrete;
using FinanceApp.MVC.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FinanceApp.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SavingController : Controller
    {
        private readonly SqlDbContext dbContext;
        private readonly UserManager<AppUser> userManager;


        public SavingController(SqlDbContext dbContext, UserManager<AppUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        [HttpGet]

        public IActionResult Index()
        {
            var user = userManager.GetUserId(this.User);
            var result = dbContext.Goals.FirstOrDefault(x => x.AppUserId == user);
            var viewModel = new GoalDTO
            {
                AppUserId = result.AppUserId,
                TargetDate = result.TargetDate,
                TargetGoal = result.TargetGoal,
                TargetStatus = result.TargetStatus,
                Type = result.Type
            };
            var userId = userManager.GetUserId(this.User);
            if (!dbContext.Saves.Where(x => x.AppUserId == userId).Any())
            {
                ViewBag.MessageSave = "To Enter a Save You must Enter The Saving Infos ";
                Save save = new Save()
                {
                    Amount = 0,
                    AppUserId = userId,
                    Type = Tip.Giris,
                    Status=false
                };
                dbContext.Saves.Add(save);
                dbContext.SaveChanges();
            }
            else
            { ViewBag.MessageSave = ""; }
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Index(GoalDTO goalDTO)
        {
            var user = userManager.GetUserId(this.User);
            var goalentity = dbContext.Goals.FirstOrDefault(x => x.AppUserId == user);
            if (ModelState.IsValid)
            {
                
                goalentity.UpdateDate = DateTime.Now;
                goalentity.TargetStatus = true;
                goalentity.TargetGoal = goalDTO.TargetGoal;
                goalentity.Type = goalDTO.Type;
                goalentity.TargetDate = goalDTO.TargetDate;
                dbContext.Goals.Update(goalentity);
                await dbContext.SaveChangesAsync();
                ViewBag.MessageSave = "";
                return RedirectToAction("saver","home",new {area=""});
            }
            
            return View(goalentity);
        }
        [HttpPost]
        public async Task<IActionResult> Cancel()
        {
            var user = userManager.GetUserId(this.User);
            var goalentity = dbContext.Goals.FirstOrDefault(x => x.AppUserId == user);
            goalentity.TargetStatus = false;
            goalentity.UpdateDate = DateTime.Now;
            goalentity.TargetGoal = null;
            goalentity.Type = null;
            goalentity.TargetDate= null;
            dbContext.Goals.Update(goalentity);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }
    }
}
