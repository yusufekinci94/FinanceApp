using FinanceApp.DAL.EntityConfigurations.Abstract;
using FinanceApp.Entities.Concrete;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.DAL.EntityConfigurations.Concrete
{
	public class CategoryConfig:BaseConfig<Category>
	{
		public override void Configure(EntityTypeBuilder<Category> builder)
		{
			base.Configure(builder);
			builder.Property(p => p.Name).HasMaxLength(100);
			builder.Property(p=>p.Description).HasMaxLength(500);
		}
	}
}
