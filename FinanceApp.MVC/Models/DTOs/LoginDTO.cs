using System.ComponentModel.DataAnnotations;

namespace FinanceApp.MVC.Models.DTOs
{
	public class LoginDTO
	{
		[Required(AllowEmptyStrings =false,ErrorMessage ="Email Required")]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "E-Mail ")]
		public string Email { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Password Required")]
		[DataType(DataType.Password)]
		[Display(Name = "Password ")]
		public string Password { get; set; }

		[Display(Name = "Remember Me")]
        public bool Persistent { get; set; }
        public bool Lock { get; set; }
    }
}
