using FinanceApp.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Entities.Concrete
{
    public class Entry : BaseEntity
    {
        public string? name { get; set; }
        public double Amount { get; set; }
        public DateTime Mounth { get; set; }
        public int TipId { get; set; }
        public Tip Tip { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        

    }
}
