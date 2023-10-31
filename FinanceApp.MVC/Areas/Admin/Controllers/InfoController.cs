using FinanceApp.DAL.Context;
using FinanceApp.Entities.Concrete;
using FinanceApp.MVC.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace FinanceApp.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InfoController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SqlDbContext dbContext;

        public InfoController(UserManager<AppUser> userManager, SqlDbContext dbContext)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index(InfoModelDTO model)
        {
            var user = userManager.GetUserId(this.User);
            var userentity = dbContext.Users.Find(user);

            model.Balance = userentity.Balance;
            model.TotalOutgoing = userentity.TotalOutgoing;
            model.TotalIncome = userentity.TotalIncome;
            model.CreditCardInterest = userentity.CreditCardInterest;
            model.Cash = userentity.Cash;
            model.CreditDebt = userentity.CreditDebt;
            model.Username = userentity.UserName;
            model.LastDay = userentity.CreditPayDay.Value.Day;
            
            model.deadline = userentity.DeadLineExecute;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Save(InfoModelDTO model)
        {
            var user = userManager.GetUserId(this.User);
            var userentity = dbContext.Users.Find(user);
            
            userentity.Balance = model.Balance;
            userentity.TotalOutgoing = model.TotalOutgoing;
            userentity.TotalIncome = model.TotalIncome;
            userentity.CreditCardInterest = model.CreditCardInterest;
            userentity.Cash = model.Cash;
            userentity.CreditDebt = model.CreditDebt;
            userentity.UserName = model.Username;
            userentity.NormalizedUserName = model.Username.ToUpper();
            userentity.DeadLineExecute = model.deadline;
            dbContext.Users.Update(userentity);
            dbContext.SaveChanges();

            return RedirectToAction("index");
        }
        [HttpPost]
        public IActionResult Day(InfoModelDTO m)
        {

            var user = userManager.GetUserId(this.User);
            var userentity = dbContext.Users.Find(user);

            userentity.CreditPayDay = new DateTime(2023, 1, m.LastDay);


            dbContext.SaveChanges();



            return RedirectToAction("index");
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = await userManager.GetUserAsync(this.User);

                if (userId != null)
                {
                    var result = await userManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        var errors = result.Errors.Select(e => e.Description).ToList();
                        var errorMessage = string.Join(", ", errors);
                        ModelState.AddModelError("", errorMessage);
                    }
                }
            }
            return View(model);
        }



    }
}
