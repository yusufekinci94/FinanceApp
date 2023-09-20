using FinanceApp.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Entities.Concrete
{
    public class Tip : BaseEntity
    {
        public string? name { get; set; }
        public ICollection<Entry> entries { get; set; }
    }
}
