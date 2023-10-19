using FinanceApp.BL.Abstract;
using FinanceApp.DAL.Repositories.Abstract;
using FinanceApp.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.BL.Concrete
{
	public class CategoryManager : BaseManager<Category>, ICategoryManager
	{
		private readonly UserManager<AppUser> userManager;
		private readonly ICategoryRepository _categoryRepository;

		public CategoryManager(ICategoryRepository repository, UserManager<AppUser> userManager) : base(repository)
		{
			this.userManager = userManager;
			this._categoryRepository = repository;
		}
	}
}
