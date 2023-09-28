using FinanceApp.Entities.Concrete;

namespace FinanceApp.MVC.Models
{
    public class EntryModel
    {
        public string? name { get; set; }
        public double Amount { get; set; }
        public int Month { get; set; } = DateTime.Now.Month;
        public Tip Type { get; set; }
        public TipPara TypeMoney { get; set; }
		public string Category { get; set; }
		public int UserId { get; set; }
    }
}
