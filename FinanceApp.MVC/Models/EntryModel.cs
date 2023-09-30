using FinanceApp.Entities.Concrete;
using Microsoft.VisualBasic;

namespace FinanceApp.MVC.Models
{
    public class EntryModel
    {
        public string? name { get; set; }
        public double Amount { get; set; }
        public int Month { get; set; } = DateTime.Now.Month;
        public Tip Type { get; set; }
        public TipPara TypeMoney { get; set; }
		public ICollection<Category> Category { get; set; }
		public int UserId { get; set; }
    }
}
