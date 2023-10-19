using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinanceApp.MVC.Models
{
	public class CategoryListModel
	{
		public int Category { get; set; } 
		public List<SelectListItem> CategoryList { get; set; }
	}
}
