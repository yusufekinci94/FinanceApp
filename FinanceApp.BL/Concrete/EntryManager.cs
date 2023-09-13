using FinanceApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceApp.DAL.Repositories.Abstract;
using FinanceApp.BL.Abstract;

namespace FinanceApp.BL.Concrete
{
    
    public class EntryManager : BaseManager<Entry>, IEntryManager
    {
        public EntryManager(IEntryRepository repository) : base(repository)
        {
        }
    }
}
