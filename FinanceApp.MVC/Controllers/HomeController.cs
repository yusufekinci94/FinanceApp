using FinanceApp.DAL.Context;
using FinanceApp.Entities.Concrete;
using FinanceApp.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FinanceApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly SqlDbContext dbContext;
		private readonly UserManager<AppUser> userManager;

		public HomeController(ILogger<HomeController> logger, SqlDbContext dbContext, UserManager<AppUser> userManager)
        {
            _logger = logger;
			this.dbContext = dbContext;
			this.userManager = userManager;
		}

        public IActionResult Index()
        {
            
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
        public IActionResult Entry()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> Entry(EntryModel m)
        {
            //Entities.Concrete.Entry entry = new Entities.Concrete.Entry();
            //entry.AppUserId = userManager.GetUserId(this.User);
            //entry.Description = m.name;
            //entry.Amount = m.Amount;
            //entry.Type = m.Type; 
            //entry.TypeMoney = m.TypeMoney;
            //entry.Categories = m.Category;
            //await dbContext.Entries.AddAsync(entry);
            return PartialView();
        }
    }
}