using FinanceApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.BL.Models
{
	public class EntriModel
	{
		public string? name { get; set; }
		public double Amount { get; set; }
		public int Month { get; set; } = DateTime.Now.Month;
		public Tip Type { get; set; }
		public TipPara TypeMoney { get; set; }
		public ICollection<Category> Category { get; set; }
		public int UserId { get; set; }
	}
}

