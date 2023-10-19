namespace FinanceApp.MVC.Models
{
    public class ConfirmModel
    {
        public int confirmCode { get; set; }
        public int confirmCodeAgain { get; set; }
        public string? AppUserId { get; set; }

    }
}
