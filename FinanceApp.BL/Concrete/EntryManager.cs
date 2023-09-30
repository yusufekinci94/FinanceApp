using FinanceApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceApp.DAL.Repositories.Abstract;
using FinanceApp.BL.Abstract;
using FinanceApp.BL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.BL.Concrete
{
    
    public class EntryManager : BaseManager<Entry>, IEntryManager
    {
		private readonly UserManager<AppUser> userManager;

		public EntryManager(IEntryRepository repository, UserManager<AppUser> userManager) : base(repository)
        {
			this.userManager = userManager;
		}
		

		public virtual async Task<int> EnterAction(EntryModel model, Entry entry)
		{

			entry.Description = model.name;
			entry.Amount = model.Amount;
			entry.Type = model.Type;
			entry.TypeMoney = model.TypeMoney;
			entry.Categories = model.Category;
			
			return await repository.Create(entry);
		}
	}
}
