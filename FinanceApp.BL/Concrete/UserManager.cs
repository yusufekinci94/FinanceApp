using FinanceApp.BL.Abstract;
using FinanceApp.DAL.Repositories.Abstract;
using FinanceApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.BL.Concrete
{
    public class UserManager : BaseManager<User>, IUserManager
    {
        public UserManager(IUserRepository repository) : base(repository)
        {
        }
    }
}
