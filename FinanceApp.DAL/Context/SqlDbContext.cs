using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinanceApp.Entities.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FinanceApp.DAL.Context
{
    public class SqlDbContext : IdentityDbContext<AppUser,IdentityRole,string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Category> Categories { get; set; }
      
       
        public SqlDbContext()
        {
        }
        public SqlDbContext(DbContextOptions<SqlDbContext> options, IHttpContextAccessor httpContextAccessor)  : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server = (localdb)\MSSQLLocalDB;Database=FinansDb; Integrated Security = true;Trust Server Certificate=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public override int SaveChanges()
        {
            var userId = GetCurrentUserId();
            var userEntities = Users.Where(e => e.Id == userId).ToList();
            foreach(var userEntity in userEntities)
            {
                userEntity.Balance = userEntity.TotalIncome - userEntity.TotalOutgoing;
            }

            return base.SaveChanges();
        }
        private string GetCurrentUserId()
        {
            
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId;
        }
    }
}
