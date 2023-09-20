namespace FinanceApp.MVC.Models
{
    public class EntryModel
    {
        public string? name { get; set; }
        public double Amount { get; set; }
        public DateTime Month { get; set; } 
        public int TipId { get; set; }
        public int UserId { get; set; }
    }
}
