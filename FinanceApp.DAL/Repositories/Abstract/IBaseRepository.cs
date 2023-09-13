using FinanceApp.DAL.Context;
using FinanceApp.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.DAL.Repositories.Abstract
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        public SqlDbContext dbContext { get; set; }
        Task<int> Create(T input);
        Task<int> Delete(T input);
        Task<int> Update(T input);

        Task<T?> GetById(int id);
        Task<ICollection<T>>? GetAll(Expression<Func<T, bool>> filter = null);
        Task<IQueryable<T>>? GetAllInclude(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[]? include);
    }
}
