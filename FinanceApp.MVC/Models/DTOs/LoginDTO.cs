using System.ComponentModel.DataAnnotations;

namespace FinanceApp.MVC.Models.DTOs
{
	public class LoginDTO
	{
		[Required(AllowEmptyStrings =false,ErrorMessage ="Email Required")]
		[DataType(DataType.EmailAddress)]
        public string Email { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Password Required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Remeber Me")]
        public bool RememberMe { get; set; }
    }
}
