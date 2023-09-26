using FinanceApp.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Entities.Concrete
{
	public class Category:BaseEntity
	{
        public string Name { get; set; }
		public string? Description { get; set; }
		public string AppUserId { get; set; }
		public AppUser User { get; set; }
		public ICollection<Entry>? Entries { get; set; }
    }
}
