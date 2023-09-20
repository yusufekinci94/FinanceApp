using FinanceApp.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Entities.Concrete
{
    public class User : BaseEntity
    {
        public double Balance { get; set; }
        public double TotalIncome { get; set; }
        public double TotalOutgoing { get; set; }
        public double MonthlyEarning { get; set; }
        public string? RepeatingOutgoings { get; set; }
        public double CreditDebt { get; set; }
        public ICollection<Entry> Entries { get; set; }
    }
}
