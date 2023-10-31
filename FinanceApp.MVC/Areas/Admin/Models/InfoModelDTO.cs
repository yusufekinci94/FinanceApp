namespace FinanceApp.MVC.Areas.Admin.Models
{
    public class InfoModelDTO
    {
        public int LastDay { get; set; }
        public double? Balance { get; set; }
        public double? TotalIncome { get; set; }
        public double? TotalOutgoing { get; set; }
        public double? Cash { get; set; }
        public double? CreditDebt { get; set; }
        public double? CreditCardInterest { get; set; }
        public string? Username { get; set; }
        public bool deadline { get; set; }
    }
}
