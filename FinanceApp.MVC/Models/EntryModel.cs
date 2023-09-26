namespace FinanceApp.MVC.Models
{
    public class EntryModel
    {
        public string? name { get; set; }
        public double Amount { get; set; }
        public int Month { get; set; } = DateTime.Now.Month;
        public string Type { get; set; }
        public int UserId { get; set; }
    }
}
