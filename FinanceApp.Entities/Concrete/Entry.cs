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
        public string Description { get; set; }
        public double Amount { get; set; }
     
        public  Tip Type { get; set; }
		public TipPara TypeMoney { get; set; }
		public string AppUserId { get; set; }
		public AppUser User { get; set; }
        public string? CategoryIds { get; set; }
        public List<Category>? Categories { get; set; }


    }
}
