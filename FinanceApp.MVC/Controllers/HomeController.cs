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
                var user = await userManager.GetUserAsync(this.User);
                if (user != null)
                {
                    return View(user); 
                }
				if (user.CreditPayDay != null)
				{
					if(DateTime.Now.Day == user.CreditPayDay.Value.Day)
					{
						user.CreditDebt = user.CreditDebt * user.CreditCardInterest;
						dbContext.Update(user);
						dbContext.SaveChanges();
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
		public async Task<IActionResult> Entry()
		{


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
				return View(m);
			}
			return View(m);
		}
		[HttpGet]
		public IActionResult CreateCategory()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> CreateCategory(CategoryDTO categoryDTO)
		{

			Category category = new Category();
			category.Description = categoryDTO.Description;
			category.Name = categoryDTO.Name;
			category.AppUserId = userManager.GetUserId(this.User);
			category.User = categoryDTO.User;


			dbContext.Categories.Add(category);

			dbContext.SaveChanges();



			return RedirectToAction("Index", "Home");
		}
	}
}