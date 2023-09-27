using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InfoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
