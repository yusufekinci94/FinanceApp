using FinanceApp.BL.Concrete;
using FinanceApp.DAL.Context;
using FinanceApp.Entities.Concrete;
using FinanceApp.MVC.Models;
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
        private readonly CategoryService categoryService;

        public EntryController(SqlDbContext dbContext, UserManager<AppUser> userManager, CategoryService categoryService)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.categoryService = categoryService;
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
            
            return View();
        }
        // add e view ekle
        [HttpPost]
        public IActionResult Add(EntryModel m)
        {

            if (!ModelState.IsValid)
            {
                return View(m);
            }
            try
            {

                Entities.Concrete.Entry entry = new Entities.Concrete.Entry();
                entry.AppUserId = userManager.GetUserId(this.User);
                //  entry.User = ?

                entry.Description = m.name;
                entry.Amount = m.Amount;
                entry.Type = m.Type;
                entry.TypeMoney = m.TypeMoney;
                if (m.checkBox == false)
                {
                    var selectedCategories = Request.Form["Categories"];
                    var categoryIdsList = new List<string>();

                    if (selectedCategories.Count > 0)
                    {
                        foreach (var categoryValue in selectedCategories)
                        {
                            var category = dbContext.Categories.FirstOrDefault(c => c.Name == categoryValue);

                            if (category != null)
                            {
                                categoryIdsList.Add(category.Name.ToString());
                            }
                        }

                        entry.Categories = m.Categories;
                        entry.CategoryIds = string.Join(",", categoryIdsList);

                        dbContext.Entries.Add(entry);
                        dbContext.SaveChanges();
                        var user = dbContext.Users.FirstOrDefault(x => x.Id == entry.AppUserId);

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

                            user.Balance = user.Cash - user.CreditDebt;



                            dbContext.SaveChanges();
                        }
                        return RedirectToAction("Index");
                    }
                }
                if (m.checkBox == true)
                {
                    Category category = new Category()
                    {
                        AppUserId = entry.AppUserId,
                        Name = m.categoryName,
                        Description = m.categoryDescription
                    };
                    dbContext.Categories.Add(category);
                    dbContext.SaveChanges();
                    entry.CategoryIds = category.Name;
                    entry.Categories = m.Categories;
                    dbContext.Entries.Add(entry);
                    dbContext.SaveChanges();
                    var user = dbContext.Users.FirstOrDefault(x => x.Id == entry.AppUserId);

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

                        user.Balance = user.Cash - user.CreditDebt;



                        dbContext.SaveChanges();
                        return RedirectToAction("Index");

                    }
                    return View(m);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An Unexcepted error occured, Please Try again");
                return View(m);
            }
            return RedirectToAction("Index");
        }

        //public IActionResult Delete(Entry entry)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(entry);
        //    }
        //    else
        //    {

        //        var user = dbContext.Users.FirstOrDefault(x => x.Id == entry.AppUserId);
        //        if (user != null)
        //        {
        //            if (entry.Type == Tip.Giris)
        //            {
        //                user.TotalIncome -= entry.Amount;
        //                if (entry.TypeMoney == TipPara.Cash)
        //                {
        //                    user.Cash -= entry.Amount;
        //                }
        //                else if (entry.TypeMoney == TipPara.Credit)
        //                {
        //                    user.CreditDebt += entry.Amount;
        //                }
        //            }
        //            else if (entry.Type == Tip.Cikis)
        //            {
        //                user.TotalOutgoing -= entry.Amount;
        //                if (entry.TypeMoney == TipPara.Cash)
        //                {
        //                    user.Cash += entry.Amount;
        //                }
        //                else if (entry.TypeMoney == TipPara.Credit)
        //                {
        //                    user.CreditDebt -= entry.Amount;
        //                }
        //            }

        //            user.Balance = user.Cash - user.CreditDebt;
        //            dbContext.Entries.Remove(entry);


        //            dbContext.SaveChanges();
        //        }
        //        dbContext.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //}
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Entry entry = new();
            entry = dbContext.Entries.FirstOrDefault(x => x.Id == id);
            var user = dbContext.Users.FirstOrDefault(x => x.Id == entry.AppUserId);
            var kategori = dbContext.Entries.Find(id);
            dbContext.Database.ExecuteSqlRaw("ALTER TABLE CategoryEntry NOCHECK CONSTRAINT FK_CategoryEntry_Entries_EntriesId");


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

                user.Balance = user.Cash - user.CreditDebt;


                dbContext.Entries.Remove(kategori);
                dbContext.SaveChanges();

                return RedirectToAction("index");
            }

            return RedirectToAction("index");
        }
    }
}
