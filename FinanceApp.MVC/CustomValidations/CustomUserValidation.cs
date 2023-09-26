using FinanceApp.Entities.Concrete;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.MVC.CustomValidations
{
	public class CustomUserValidation : IUserValidator<AppUser>
	{
		public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
		{
			List<IdentityError> errors = new List<IdentityError>();

			if (int.TryParse(user.UserName[0].ToString(), out int _)) //Kullanıcı adının sayısal ifadeyle başlamaması kontrolü
				errors.Add(new IdentityError { Code = "UserNameNumberStartWith", Description = "Username Can not Start With Numeric Digits" });
			if (user.UserName.Length < 3 && user.UserName.Length > 25) //Kullanıcı adının 3 ile 25 karakter arasında olması
				errors.Add(new IdentityError { Code = "UserNameLength", Description = "Username must be between 3-25 characters" });
			if (user.Email.Length > 70) // Emailin 70 karakterden fazla olmaması kontrolü
				errors.Add(new IdentityError { Code = "EmailLength", Description = "E-mail Can not be longer than 70 characters" });

			if (!errors.Any())
				return Task.FromResult(IdentityResult.Success);
			return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
		}
	}
}
