using FinanceApp.Entities.Concrete;

namespace FinanceApp.MVC.Models.DTOs
{
    public class MonthlyDTO
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public int PaymentDay { get; set; }
        public int Installment { get; set; }
        public string? AppUserId { get; set; }
        public bool Status { get; set; }
        public Tip Type { get; set; }
        public TipPara TypeMoney { get; set; }
        public string? Categories { get; set; }
        public bool checkBox { get; set; }
        public bool checkBox2 { get; set; }
        public string? categoryName { get; set; }
        public string? categoryDescription { get; set; }
    }
}
