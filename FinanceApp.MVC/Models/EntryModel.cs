using FinanceApp.Entities.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public string? CategoryIds { get; set; }
        public List<Category>? Categories { get; set; }
        public double? Cash { get; set; }
        public double? CashMinus { get; set; }
        public double? CreditDebt { get; set; }
        public double? CreditDebtMinus { get; set; }
		public int UserId { get; set; }
        public bool checkBox { get; set; }
        public string? categoryName { get; set; }
        public string? categoryDescription { get; set; }
        public double? BankAction { get; set; }
        public Tip? BankType { get; set; }

    }

}
