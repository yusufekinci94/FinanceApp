using FinanceApp.Entities.Concrete;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.MVC.CustomValidations
{
	public class CustomPasswordValidation : IPasswordValidator<AppUser>
	{
		public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
		{
			List<IdentityError> errors = new List<IdentityError>();
			if (password.Length < 8)
				errors.Add(new IdentityError { Code = "PasswordLength", Description = "Password Minimum 8 Characters" });
			if (password.ToLower().Contains(user.UserName.ToLower())) 
				errors.Add(new IdentityError { Code = "PasswordContainsUserName", Description = "Password Can not Contain Username" });
			if (!errors.Any()) 
				return Task.FromResult(IdentityResult.Success);
			else 
				return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
		}
	}
}
