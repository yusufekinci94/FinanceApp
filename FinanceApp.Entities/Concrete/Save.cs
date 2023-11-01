using FinanceApp.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Entities.Concrete
{
    public class Save:BaseEntity
    {
        public string AppUserId { get; set; }
        public Tip Type { get; set; }
        public double Amount { get; set; }
        public bool Status { get; set; }

    }
}
