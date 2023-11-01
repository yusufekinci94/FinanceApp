﻿using FinanceApp.Entities.Concrete;

namespace FinanceApp.MVC.Models.DTOs
{
    public class UsersDTO
    {
        public double? Balance { get; set; }
        public double? TotalIncome { get; set; }
        public double? TotalOutgoing { get; set; }
        public double? MonthlyEarning { get; set; }
        public double? CreditDebt { get; set; }
        public double? Cash { get; set; }
        public double? CreditCardInterest { get; set; }
        public DateTime? CreditPayDay { get; set; }
        public ICollection<Entry> Entries { get; set; }
        public bool HasExecutedForThisMonth { get; set; }
        public bool DeadLineExecute { get; set; }
        public double? TargetGoal { get; set; }
        public DateTime? TargetDate { get; set; }
        public Tip? Type { get; set; }
        public bool TargetStatus { get; set; }
    }
}
