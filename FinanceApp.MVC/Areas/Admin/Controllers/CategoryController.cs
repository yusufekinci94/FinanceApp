using FinanceApp.DAL.Context;
using FinanceApp.Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FinanceApp.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly SqlDbContext dbContext;
        private readonly UserManager<AppUser> userManager;

        public CategoryController(SqlDbContext dbContext, UserManager<AppUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            var user = userManager.GetUserId(this.User);
            var result = dbContext.Categories.Where(x=>x.AppUserId==user).ToList();
            return View(result);
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var kategori = dbContext.Categories.Find(id);


            return View(kategori);
        }

        [HttpPost]
        public IActionResult Update(Category kategori)
        {
            if (!ModelState.IsValid)
            {
                return View(kategori);
            }
            else
            {
                dbContext.Categories.Update(kategori);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public IActionResult Add()
        {
            Category kategori = new();
            return View(kategori);
        }

        [HttpPost]
        public IActionResult Add(Category kategori)
        {

            if (!ModelState.IsValid)
            {
                return View(kategori);
            }
            try
            {
                kategori.AppUserId=userManager.GetUserId(this.User);
                dbContext.Categories.Add(kategori);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An Unexcepted error occured, Please Try again");
                return View(kategori);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(Category kategori)
        {
            if (!ModelState.IsValid)
            {
                return View(kategori);
            }
            else
            {
                dbContext.Categories.Remove(kategori);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        public IActionResult Delete(int id)
        {
            var kategori = dbContext.Categories.Find(id);


            return View(kategori);
        }
    }
}
