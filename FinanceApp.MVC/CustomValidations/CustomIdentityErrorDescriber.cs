using Microsoft.AspNetCore.Identity;

namespace FinanceApp.MVC.CustomValidations
{
	public class CustomIdentityErrorDescriber : IdentityErrorDescriber
	{
		public override IdentityError DuplicateUserName(string userName) => new IdentityError { Code = "DuplicateUserName", Description = $"\"{userName}\" is used by another user" };
		public override IdentityError InvalidUserName(string userName) => new IdentityError { Code = "InvalidUserName", Description = "invalid user name" };
		public override IdentityError DuplicateEmail(string email) => new IdentityError { Code = "DuplicateEmail", Description = $"\"{email}\" is used by another user" };
		public override IdentityError InvalidEmail(string email) => new IdentityError { Code = "InvalidEmail", Description = "Invalid Email." };
	}
}
