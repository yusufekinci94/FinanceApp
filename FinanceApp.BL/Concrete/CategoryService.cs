using FinanceApp.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.BL.Concrete
{

    public class CategoryService
    {
        private readonly SqlDbContext _dbContext;

        public CategoryService(SqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<string> GetCategoryNames(string categoryIds)
        {
            if (string.IsNullOrEmpty(categoryIds))
            {
                return new List<string>();
            }

            var categoryIdsList = categoryIds.Split(',').Select(int.Parse).ToList();
            var categoryNames = _dbContext.Categories
                .Where(c => categoryIdsList.Contains(c.Id))
                .Select(c => c.Name)
                .ToList();

            return categoryNames;
        }
    }

}
