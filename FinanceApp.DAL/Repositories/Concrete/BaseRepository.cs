﻿using FinanceApp.DAL.Context;
using FinanceApp.DAL.Repositories.Abstract;
using FinanceApp.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.DAL.Repositories.Concrete
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        public SqlDbContext dbContext { get; set; }

        public BaseRepository()
        {
            dbContext = new SqlDbContext();
        }

        public async Task<int> Create(T input)
        {
            await dbContext.Set<T>().AddAsync(input);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(T input)
        {
            dbContext.Set<T>().Remove(input);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> Update(T input)
        {
            dbContext.Set<T>().Update(input);
            return await dbContext.SaveChangesAsync();
        }
        public async Task<T?> GetById(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<ICollection<T>>? GetAll(Expression<Func<T, bool>> filter = null)
        {
            if (filter == null)
            {
                return await dbContext.Set<T>().ToListAsync();
            }
            else
            {
                return await dbContext.Set<T>().Where(filter).ToListAsync();
            }
        }

        public async Task<IQueryable<T>>? GetAllInclude(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[]? include)
        {
            IQueryable<T> query;
            query = dbContext.Set<T>();
            if (filter != null)
            {
                query = dbContext.Set<T>().Where(filter);
            }
            return include.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}

