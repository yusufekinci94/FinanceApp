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
    public class EntryController : Controller
    {
        private readonly SqlDbContext dbContext;
        private readonly UserManager<AppUser> userManager;

        public EntryController(SqlDbContext dbContext, UserManager<AppUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            var user = userManager.GetUserId(this.User);
            var result = dbContext.Entries.Where(x => x.AppUserId == user).ToList();
            return View(result);

        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var entry = dbContext.Entries.Find(id);


            return View(entry);
        }

        [HttpPost]
        public IActionResult Update(Entry entry)
        {
            if (!ModelState.IsValid)
            {
                return View(entry);
            }
            else
            {
                dbContext.Entries.Update(entry);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public IActionResult Add()
        {
            Entry entry = new();
            return View(entry);
        }

        [HttpPost]
        public IActionResult Add(Entry entry)
        {

            if (!ModelState.IsValid)
            {
                return View(entry);
            }
            try
            {
                var user = dbContext.Users.FirstOrDefault(x => x.Id == entry.AppUserId);
                entry.AppUserId = userManager.GetUserId(this.User);
                dbContext.Entries.Add(entry);
                dbContext.SaveChanges();
                if (user != null)
                {
                    if (entry.Type == Tip.Giris)
                    {
                        user.TotalIncome += entry.Amount;
                        if (entry.TypeMoney == TipPara.Cash)
                        {
                            user.Cash += entry.Amount;
                        }
                        else if (entry.TypeMoney == TipPara.Credit)
                        {
                            user.CreditDebt -= entry.Amount;
                        }
                    }
                    else if (entry.Type == Tip.Cikis)
                    {
                        user.TotalOutgoing += entry.Amount;
                        if (entry.TypeMoney == TipPara.Cash)
                        {
                            user.Cash -= entry.Amount;
                        }
                        else if (entry.TypeMoney == TipPara.Credit)
                        {
                            user.CreditDebt += entry.Amount;
                        }
                    }

                    user.Balance = user.TotalIncome - user.TotalOutgoing;

                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An Unexcepted error occured, Please Try again");
                return View(entry);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(Entry entry)
        {
            if (!ModelState.IsValid)
            {
                return View(entry);
            }
            else
            {
                dbContext.Entries.Remove(entry);
                var user = dbContext.Users.FirstOrDefault(x => x.Id == entry.AppUserId);
                if (user != null)
                {
                    if (entry.Type == Tip.Giris)
                    {
                        user.TotalIncome -= entry.Amount;
                        if (entry.TypeMoney == TipPara.Cash)
                        {
                            user.Cash -= entry.Amount;
                        }
                        else if (entry.TypeMoney == TipPara.Credit)
                        {
                            user.CreditDebt += entry.Amount;
                        }
                    }
                    else if (entry.Type == Tip.Cikis)
                    {
                        user.TotalOutgoing -= entry.Amount;
                        if (entry.TypeMoney == TipPara.Cash)
                        {
                            user.Cash += entry.Amount;
                        }
                        else if (entry.TypeMoney == TipPara.Credit)
                        {
                            user.CreditDebt -= entry.Amount;
                        }
                    }

                    user.Balance = user.TotalIncome - user.TotalOutgoing;



                    dbContext.SaveChanges();
                }
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        public IActionResult Delete(int id)
        {
            var kategori = dbContext.Entries.Find(id);


            return View(kategori);
        }
    }
}
