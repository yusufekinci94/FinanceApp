﻿using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}