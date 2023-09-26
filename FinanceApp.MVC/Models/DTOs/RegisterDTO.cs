using System.ComponentModel.DataAnnotations;

namespace FinanceApp.MVC.Models.DTOs
{
    public class RegisterDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Required")]
        [MinLength(3, ErrorMessage = "Password Minimum 3 Characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage ="Passwords Must Match")]
		public string PasswordRepeat { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Username Required")]
        [MinLength(3,ErrorMessage = "Username Minimum 3 Characters")]
        [DataType(DataType.Text)]
        public string Username { get; set; }
        
        
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
    }
}
