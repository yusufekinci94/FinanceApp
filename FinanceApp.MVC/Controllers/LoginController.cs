using FinanceApp.BL.Concrete;
using FinanceApp.DAL.Context;
using FinanceApp.Entities.Concrete;
using FinanceApp.MVC.Models;
using FinanceApp.MVC.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Net.Mail;

namespace FinanceApp.MVC.Controllers
{
	public class LoginController : Controller
	{
		private readonly UserManager<AppUser> userManager;
        private readonly SqlDbContext dbContext;
        private readonly SignInManager<AppUser> signInManager;
		private readonly RoleManager<IdentityRole> roleManager;
       



        public LoginController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,SqlDbContext sqlDbContext,RoleManager<IdentityRole> roleManager)
        {
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.roleManager = roleManager;
			this.dbContext = sqlDbContext;
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
		[HttpPost]
		public async Task<IActionResult> Login(LoginDTO loginDTO)
		{
			if (ModelState.IsValid)
			{
				AppUser user = await userManager.FindByEmailAsync(loginDTO.Email);
				if (user != null)
				{
					await signInManager.SignOutAsync();
					Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, loginDTO.Password, loginDTO.Persistent, loginDTO.Lock);
					if (result.Succeeded)
					{
						await userManager.ResetAccessFailedCountAsync(user);
						//return RedirectToAction("Index", "Home");// BURAYA NE YAPILABİLİR BİR SOR
						return RedirectToAction("Index", "Home", new { Area = "" });

                    }
					else
					{
						await userManager.AccessFailedAsync(user); //Eğer ki başarısız bir account girişi söz konusu ise AccessFailedCount kolonundaki değer +1 arttırılacaktır. 

						int failcount = await userManager.GetAccessFailedCountAsync(user); //Kullanıcının yapmış olduğu başarısız giriş deneme adedini alıyoruz.
						if (failcount == 3)
						{
							await userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(1))); //Eğer ki başarısız giriş denemesi 3'ü bulduysa ilgili kullanıcının hesabını kilitliyoruz.
							ModelState.AddModelError("Locked", "Art arda 3 başarısız giriş denemesi yaptığınızdan dolayı hesabınız 1 dk kitlenmiştir.");
						}
						else
						{
							if (result.IsLockedOut)
								ModelState.AddModelError("Locked", "Art arda 3 başarısız giriş denemesi yaptığınızdan dolayı hesabınız 1 dk kilitlenmiştir.");
							else
								ModelState.AddModelError("NotUser2", "E-posta veya şifre yanlış.");
						}


					}
				}
				else
				{
					ModelState.AddModelError("NotUser", "There is not a username like this");
					ModelState.AddModelError("NotUser2", "Email or Password is incorrect");
				}

			}
			return View(loginDTO);
			


		}
		[HttpGet]
		public async Task<IActionResult> Register()
		{
			RegisterDTO registerDTO = new RegisterDTO();
			return View(registerDTO);
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterDTO registerDTO)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToAction("Register");
			}
			else
			{
				AppUser myUser = new AppUser
				{
					UserName = registerDTO.Username,
					Email = registerDTO.Email,
					PhoneNumber = registerDTO.PhoneNumber,
					Balance = 0,
					Cash = 0,
					CreditDebt = 0,
					TotalIncome = 0,
					TotalOutgoing = 0,
					MonthlyEarning = 0,
					CreditCardInterest = 3.26,
					DeadLineExecute = false,
					CreditPayDay = DateTime.Now
					
					
					
					
					
				};
				IdentityResult result = await userManager.CreateAsync(myUser,registerDTO.Password);
				
				if(result.Succeeded)
				{

					var identityresult = await userManager.AddToRoleAsync(myUser, "Admin");
					

					return RedirectToAction("Confirmation",myUser);
				}
				else
				{
					result.Errors.ToList().ForEach(error => ModelState.AddModelError(error.Code, error.Description));
				}
			}
			return View();
		}
        [HttpGet]
        public async Task<IActionResult> Confirmation(AppUser appUser)
        {
            Random random = new Random();
            int confirmCode = random.Next(100000, 1000000);
			ConfirmModel confirmModel = new ConfirmModel();
			confirmModel.confirmCode = confirmCode;
			confirmModel.AppUserId = appUser.Id;
            var smtpClient = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                UseDefaultCredentials = false,
                EnableSsl = true,
            };
            smtpClient.Credentials = new NetworkCredential("financeappacunmedya@hotmail.com", "yusufekinci94");
			var mailMessage = new MailMessage
			{
				From = new MailAddress("financeappacunmedya@hotmail.com"),
				Subject = "Confirmation Email",
				Body = "Confirmation Code: " + confirmCode,
                IsBodyHtml = true,
            };
			mailMessage.To.Add(appUser.Email);
			smtpClient.Send(mailMessage);
            Goal goal = new Goal()
            {
                AppUserId = confirmModel.AppUserId,
                TargetStatus = false

            };
            dbContext.Goals.Add(goal);
            dbContext.SaveChanges();
            return View(confirmModel);
        }
        [HttpPost]
        public async Task<IActionResult> Confirmation(ConfirmModel confirmModel)
        {
			if (confirmModel.confirmCode == confirmModel.confirmCodeAgain)
			{
				var user = userManager.Users.FirstOrDefault(x => x.Id == confirmModel.AppUserId);
				if (user != null)
				{
					user.EmailConfirmed = true;
				}
				dbContext.SaveChanges();
				

				return RedirectToAction("Login");
			}
            return View(confirmModel);
        }

        public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Index","Home");
		}


	}	
}
