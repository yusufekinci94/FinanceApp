using AutoMapper;
using FinanceApp.BL.Concrete;
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
		private readonly IMapper mapper;
		private readonly EntryManager entryManager;

		public HomeController(ILogger<HomeController> logger, SqlDbContext dbContext, UserManager<AppUser> userManager,IMapper mapper,EntryManager entryManager)
        {
            _logger = logger;
			this.dbContext = dbContext;
			this.userManager = userManager;
			this.mapper = mapper;
			this.entryManager = entryManager;
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
            
            Entities.Concrete.Entry entry = mapper.Map<Entry>(m);
            entry.AppUserId = userManager.GetUserId(this.User);
            //  entry.User = ?
            //    entry.Description = m.name;
            //    entry.Amount = m.Amount;
            //    entry.Type = m.Type;
            //    entry.TypeMoney = m.TypeMoney;
            //    entry.Categories = m.Category;
            //    await dbContext.Entries.AddAsync(entry);

            entryManager.Create(entry);
            return PartialView();
        }
    }
}