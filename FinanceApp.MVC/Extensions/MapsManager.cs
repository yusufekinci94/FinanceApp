using FinanceApp.BL.Concrete;
using FinanceApp.BL.Models;
using FinanceApp.DAL.Repositories.Abstract;
using FinanceApp.DAL.Repositories.Concrete;
using FinanceApp.MVC.Models;

namespace FinanceApp.MVC.Extensions
{
	public static class MapsManager
	{

		public static IServiceCollection AddAcunManagerService(this IServiceCollection services)
		{
			services.AddScoped<IEntryRepository,EntryRepository>();
			services.AddScoped<ICategoryRepository,CategoryRepository>();
			services.AddScoped<EntriModel,EntryModel>();
			services.AddScoped<EntryModel, EntriModel>();
			
			

			return services;
		}
	}
}
