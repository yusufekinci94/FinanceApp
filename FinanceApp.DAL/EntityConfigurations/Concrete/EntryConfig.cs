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
	public class EntryConfig : BaseConfig<Entry>
	{
		public override void Configure(EntityTypeBuilder<Entry> builder)
		{
			base.Configure(builder);
			builder.Property(p=>p.Description).HasMaxLength(255);
		}
	}
}
