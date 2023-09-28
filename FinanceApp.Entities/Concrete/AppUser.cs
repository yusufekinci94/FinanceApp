using FinanceApp.Entities.Abstract;

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Entities.Concrete
{
    public class AppUser : IdentityUser
    {
        public double? Balance { get; set; }
        public double? TotalIncome { get; set; }
        public double? TotalOutgoing { get; set; }
        public double? MonthlyEarning { get; set; }
        public double? CreditDebt { get; set; }
        public double? Cash { get; set; }
      
        public ICollection<Entry> Entries { get; set; }
    }
}
