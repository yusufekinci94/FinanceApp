using AutoMapper;
using FinanceApp.BL.Abstract;
using FinanceApp.BL.Concrete;
using FinanceApp.DAL.Context;
using FinanceApp.Entities.Concrete;
using FinanceApp.MVC.Models;
using FinanceApp.MVC.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace FinanceApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SqlDbContext dbContext;
        private readonly UserManager<AppUser> userManager;
        private readonly ICategoryManager categoryManager;
        private readonly IMapper mapper;
        private readonly CategoryService categoryService;


        public HomeController(ILogger<HomeController> logger, SqlDbContext dbContext, UserManager<AppUser> userManager, ICategoryManager categoryManager, IMapper mapper,
            CategoryService categoryService)
        {
            _logger = logger;
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.categoryManager = categoryManager;
            this.mapper = mapper;
            this.categoryService = categoryService;


        }

        public async Task<IActionResult> Index()
        {
            if (userManager.GetUserId(this.User) != null)
            {
                var userid = await userManager.GetUserAsync(this.User);
                if (userid != null)
                {
                    return View(userid);
                }
                var users = dbContext.Users.ToList();
                foreach(var user in users) {
                    if (user.CreditPayDay != null && user.DeadLineExecute == true)
                    {
                        // Eğer bu ayın günü CreditPayDay gününe eşitse ve daha önce bu ay için işlem yapılmadıysa, işlemi yap.
                        if (DateTime.Now.Day == user.CreditPayDay.Value.Day && !user.HasExecutedForThisMonth)
                        {
                            user.CreditDebt = user.CreditDebt * user.CreditCardInterest;
                            user.HasExecutedForThisMonth = true;
                            dbContext.Update(user);
                            dbContext.SaveChanges();
                        }
                    }
                }
                
            }
            return View();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public async Task<IActionResult> CreateCategory()
        {
            var userId = userManager.GetUserId(this.User);
            if (!dbContext.Categories.Where(x => x.AppUserId == userId).Any())
            {
                ViewBag.Message = "Please enter at least 1 category to enter a entry";
            }
            else
            { ViewBag.Message = ""; }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDTO category)
        {
            
            //if (category.Description!=null&&category.Name!=null)
            if(ModelState.IsValid)
            {
                Entities.Concrete.Category _category = new Entities.Concrete.Category();
                _category.AppUserId = userManager.GetUserId(this.User);
                _category.Name = category.Name;
                _category.Description = category.Description;
                dbContext.Categories.Add(_category);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpGet]
        public async Task<IActionResult> Entry()
        {
            var userId = userManager.GetUserId(this.User);
            if (!dbContext.Categories.Where(x => x.AppUserId == userId).Any())
            {
                return RedirectToAction("createcategory");
            }

            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Entry(EntryModel m)
        {
            if (ModelState.IsValid)
            {
                Entities.Concrete.Entry entry = new Entities.Concrete.Entry();
                entry.AppUserId = userManager.GetUserId(this.User);
                //  entry.User = ?

                entry.Description = m.name;
                entry.Amount = m.Amount;
                entry.Type = m.Type;
                entry.TypeMoney = m.TypeMoney;
                entry.UpdateDate = DateTime.Now;
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
                        return RedirectToAction("Index", "Home");
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
                        return RedirectToAction("Index", "Home");

                    }
                    return View(m);
                }
                if (!ModelState.IsValid)
                {
                    List<string> errorMessages = new List<string>();

                    
                    foreach (var modelState in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                           
                            errorMessages.Add(error.ErrorMessage);
                        }
                    }

                    
                }

            }
            return View(m);
        }
        [HttpPost]
        public IActionResult Cash(EntryModel m)
        {
            if (m.Cash != null)
            {
                var user = userManager.GetUserId(this.User);
                var currentUser = dbContext.Users.FirstOrDefault(x => x.Id == user);

                if (currentUser != null)
                {
                    currentUser.Cash += m.Cash;

                    dbContext.Update(currentUser);
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction("entry");
        }
        [HttpPost]
        public IActionResult CashMinus(EntryModel m)
        {
            if (m.CashMinus != null)
            {
                var user = userManager.GetUserId(this.User);
                var currentUser = dbContext.Users.FirstOrDefault(x => x.Id == user);

                if (currentUser != null)
                {
                    currentUser.Cash -= m.CashMinus;

                    dbContext.Update(currentUser);
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction("entry");
        }
        [HttpPost]
        public IActionResult Credit(EntryModel m)
        {
            if (m.CreditDebt != null)
            {
                var user = userManager.GetUserId(this.User);
                var currentUser = dbContext.Users.FirstOrDefault(x => x.Id == user);

                if (currentUser != null)
                {
                    currentUser.CreditDebt += m.CreditDebt;

                    dbContext.Update(currentUser);
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction("entry");
        }
        [HttpPost]
        public IActionResult CreditMinus(EntryModel m)
        {
            if (m.CreditDebtMinus != null)
            {
                var user = userManager.GetUserId(this.User);
                var currentUser = dbContext.Users.FirstOrDefault(x => x.Id == user);

                if (currentUser != null)
                {
                    currentUser.CreditDebt -= m.CreditDebtMinus;

                    dbContext.Update(currentUser);
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction("entry");
        }
        //    [HttpGet]
        //    public IActionResult CreateCategory()
        //    {
        //        return View();
        //    }
        //    [HttpPost]
        //    public async Task<IActionResult> CreateCategory(CategoryDTO categoryDTO)
        //    {

        //        Category category = new Category();
        //        category.Description = categoryDTO.Description;
        //        category.Name = categoryDTO.Name;
        //        category.AppUserId = userManager.GetUserId(this.User);
        //        category.User = categoryDTO.User;


        //        dbContext.Categories.Add(category);

        //        dbContext.SaveChanges();



        //        return RedirectToAction("Index", "Home");
        //    }
    }
}