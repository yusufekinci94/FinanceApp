using FinanceApp.DAL.Repositories.Abstract;
using FinanceApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.DAL.Repositories.Concrete
{
    public class EntryRepository : BaseRepository<Entry>, IEntryRepository
    {
    }
}
