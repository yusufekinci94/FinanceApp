using FinanceApp.BL.Concrete;
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

			
			

			return services;
		}
	}
}
