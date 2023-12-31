﻿using System;
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
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Save> Saves { get; set; }
        public DbSet<Monthly> Monthlies { get; set; }
      
       
        public SqlDbContext()
        {
        }
        public SqlDbContext(DbContextOptions<SqlDbContext> options, IHttpContextAccessor httpContextAccessor)  : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=104.247.167.130\MSSQLSERVER2017;User ID=yusufca5_financeapp;Password=rL6z321%v;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
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
                userEntity.Balance = userEntity.Cash - userEntity.CreditDebt;
            }
            Database.ExecuteSqlRaw("ALTER TABLE CategoryEntry NOCHECK CONSTRAINT FK_CategoryEntry_Entries_EntriesId");
            var result = base.SaveChanges();
            

            return result;
        }
        private string GetCurrentUserId()
        {

            if (_httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated == true)
            {
                return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            return null; 
        }
    }
}
