using AutoMapper;
using FinanceApp.BL.Abstract;
using FinanceApp.BL.Concrete;
using FinanceApp.DAL.Context;
using FinanceApp.Entities.Concrete;
using FinanceApp.MVC.Models;
using FinanceApp.MVC.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
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
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (this.User.Identity.IsAuthenticated == true)
            {
                var userx = await userManager.GetUserAsync(this.User);
                if (userx.EmailConfirmed == false)
                {
                    return RedirectToAction("confirmation", "login");
                }
            }
            if (userManager.GetUserId(this.User) != null)
            {
                UsersDTO usersDTO = new UsersDTO();
                var userid = await userManager.GetUserAsync(this.User);
                if (userid != null)
                {
                    var usersEntity = dbContext.Users.Find(userid.Id);
                    var goalEntity = dbContext.Goals.FirstOrDefault(x => x.AppUserId == userid.Id);
                    usersDTO.Balance = usersEntity.Balance;
                    usersDTO.Cash = usersEntity.Cash;
                    usersDTO.TotalIncome = usersEntity.TotalIncome;
                    usersDTO.TotalOutgoing = usersEntity.TotalOutgoing;
                    usersDTO.CreditDebt = usersEntity.CreditDebt;
                    usersDTO.CreditPayDay = usersEntity.CreditPayDay;
                    if (goalEntity != null)
                    {
                        if (goalEntity.TargetGoal != null && goalEntity.TargetDate != null)
                        {
                            usersDTO.TargetGoal = goalEntity.TargetGoal;
                            usersDTO.TargetDate = goalEntity.TargetDate;
                            usersDTO.Type = goalEntity.Type;
                            usersDTO.TargetStatus = goalEntity.TargetStatus;
                        }
                    }
                    else
                    {
                        usersDTO.TargetGoal = null;
                        usersDTO.TargetDate = null;
                        usersDTO.Type = null;
                        usersDTO.TargetStatus = false;
                    }

                    return View(usersDTO);
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
        [Authorize(Roles = "Admin")]
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
            if (ModelState.IsValid)
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
        [Authorize(Roles = "Admin")]
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
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMonthly()
        {
            var userid = userManager.GetUserId(this.User);
            var categories = dbContext.Categories.Where(x => x.AppUserId == userid).ToList();
            var categoryNames = string.Join(", ", categories.Select(c => c.Name));


            var viewModel = new MonthlyDTO
            {
                AppUserId=userid,
                Categories = categoryNames
            };

            return View(viewModel);
        }



        [HttpPost]
        public async Task<IActionResult> CreateMonthly(MonthlyDTO m)
        {
            if (ModelState.IsValid)
            {
                Monthly monthly = new Monthly()
                {
                    AppUserId = userManager.GetUserId(this.User),
                    Amount = m.Amount,
                    Installment = m.Installment,
                    Name = m.Name,
                    Status = true,
                    Type = m.Type,
                    TypeMoney = m.TypeMoney,
                    PaymentDay = m.PaymentDay,
                    Categories = m.Categories,

                };
                if (m.Installment == 0)
                {
                    monthly.Installment = 999999999;
                }
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
                        monthly.Categories = string.Join(",", categoryIdsList);
                        m.Categories = monthly.Categories;
                        dbContext.Monthlies.Add(monthly);
                        dbContext.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
                if (m.checkBox == true)
                {
                    Category category = new Category()
                    {
                        AppUserId = monthly.AppUserId,
                        Name = m.categoryName,
                        Description = m.categoryDescription
                    };
                    await dbContext.Categories.AddAsync(category);
                    await dbContext.SaveChangesAsync();
                    monthly.Categories = m.categoryName;
                    await dbContext.Monthlies.AddAsync(monthly);
                    await dbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            if (!ModelState.IsValid)
            {

                var userid = userManager.GetUserId(this.User);
                var categories = dbContext.Categories.Where(x => x.AppUserId == userid).ToList();
                var categoryNames = string.Join(", ", categories.Select(c => c.Name));
                m.Categories = categoryNames;
                List<string> errorMessages = new List<string>();


                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {

                        errorMessages.Add(error.ErrorMessage);
                    }
                }
              
                return View(m);
            }
            return View(m);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Saver()
        {
            var userId = userManager.GetUserId(this.User);
            var goalEntity = dbContext.Goals.FirstOrDefault(x => x.AppUserId == userId);
            SaverDTO saverDTO = new SaverDTO();
            if (!dbContext.Saves.Where(x => x.AppUserId == userId).Any())
            {
                return RedirectToAction("index", "saving", new { area = "admin" });
            }
            else
            {

                if (dbContext.Saves.Where(x => (x.AppUserId == userId) && (x.Status == true)).Any())
                {
                    saverDTO = new SaverDTO()
                    {
                        AppUserId = userId,
                        TargetGoal = goalEntity.TargetGoal,
                        TargetDate = goalEntity.TargetDate,
                        TargetStatus = goalEntity.TargetStatus,
                        Status = false,
                        Type = goalEntity.Type,
                        SaverAmount = 0,
                        SaverType = null
                    };
                }
                else
                {
                    saverDTO = new SaverDTO()
                    {
                        AppUserId = userId,
                        TargetGoal = null,
                        TargetDate = null,
                        TargetStatus = false,
                        Status = false,
                        Type = null,
                        SaverAmount = 0,
                        SaverType = null

                    };
                }
                return View(saverDTO);
            }

        }
        [HttpPost]
        public async Task<IActionResult> Saver(SaverDTO saverDto)
        {
            if (ModelState.IsValid)
            {
                var userId = userManager.GetUserId(this.User);
                var goalEntity = dbContext.Goals.FirstOrDefault(x => x.AppUserId == userId);

                Save save = new Save
                {
                    Amount = saverDto.SaverAmount,
                    AppUserId = saverDto.AppUserId,
                    Status = true,
                    Type = saverDto.SaverType.Value,
                };
                dbContext.Saves.Add(save);
                dbContext.SaveChanges();
                if (saverDto.SaverType.Value == Tip.Giris)
                {
                    goalEntity.TargetGoal = goalEntity.TargetGoal + saverDto.SaverAmount;
                    dbContext.Goals.Update(goalEntity);
                    dbContext.SaveChanges();
                }
                else if (saverDto.SaverType.Value == Tip.Cikis)
                {
                    goalEntity.TargetGoal = goalEntity.TargetGoal - saverDto.SaverAmount;
                    dbContext.Goals.Update(goalEntity);
                    dbContext.SaveChanges();
                }
                return RedirectToAction("saver", "home");
            }
            else
            {
                return View(saverDto);
            }

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
        [HttpPost]
        public IActionResult BankAction(EntryModel m)
        {
            if (m.BankAction != null&&m.BankType!=null)
            {
                var user = userManager.GetUserId(this.User);
                var currentUser = dbContext.Users.FirstOrDefault(x => x.Id == user);

                if (currentUser != null)
                {
                    if (m.BankType == Tip.Giris)
                    {
                        currentUser.CreditDebt += m.BankAction;
                        currentUser.Cash += m.BankAction;
                        dbContext.Update(currentUser);
                        dbContext.SaveChanges();
                    }
                    if (m.BankType == Tip.Cikis)
                    {
                        currentUser.CreditDebt -= m.BankAction;
                        currentUser.Cash -= m.BankAction;
                        dbContext.Update(currentUser);
                        dbContext.SaveChanges();
                    }

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