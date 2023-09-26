using FinanceApp.Entities.Concrete;
using FinanceApp.MVC.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace FinanceApp.MVC.Controllers
{
	public class LoginController : Controller
	{
		private readonly UserManager<AppUser> userManager;
		private readonly SignInManager<AppUser> signInManager;
		private readonly RoleManager<IdentityRole> roleManager;

		public LoginController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.roleManager = roleManager;
		}
        public IActionResult Index()
		{
			return View();
		}
		[HttpGet]
		public async Task<IActionResult> Login()
		{
			LoginDTO loginDTO = new LoginDTO();
			return View(loginDTO);
		}
		//[HttpPost]
		//public async Task<IActionResult> Login(LoginDTO loginDTO)
		//{
			
		//}
		[HttpGet]
		public async Task<IActionResult> Register()
		{
			RegisterDTO registerDTO = new RegisterDTO();
			return View(registerDTO);
		}

		//[HttpPost]
		//public async Task<IActionResult> Register(RegisterDTO registerDTO)
		//{
		//	if(registerDTO.Password != registerDTO.PasswordRepeat) 
		//	{
		//		return RedirectToAction("Register");
		//	}
		//	else
		//	{

		//	}
		//}





	}	
}
