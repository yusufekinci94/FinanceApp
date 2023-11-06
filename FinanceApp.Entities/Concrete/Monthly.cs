using FinanceApp.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Entities.Concrete
{
    public class Monthly:BaseEntity
    {
        public string Name { get; set; }   
        public double Amount { get; set; }
        public int PaymentDay { get; set; }
        public int Installment { get; set; }
        public string AppUserId { get; set; }
        public bool Status { get; set; }
        public Tip Type { get; set; }
        public TipPara TypeMoney { get; set; }
        public string? Categories{ get; set; }
        
        

    }
}
