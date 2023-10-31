using FinanceApp.BL.Abstract;
using FinanceApp.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace FinanceApp.BL.Concrete
{
    public class MonthlyTask : IMonthlyTask
    {
        private System.Timers.Timer timer;
        public MonthlyTask()
        {
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);


            DateTime now = DateTime.Now;
            DateTime nextMonth = new DateTime(now.Year, now.Month, 1).AddMonths(1);
            TimeSpan timeToNextMonth = nextMonth - now;
            timer.Interval = timeToNextMonth.TotalMilliseconds;

            timer.Start();
        }
        public void Start()
        {
            timer.Start();
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            using (var dbContext = new SqlDbContext())
            {
                var users = dbContext.Users.ToList();
                foreach (var user in users)
                {
                    user.HasExecutedForThisMonth = false;
                    dbContext.Users.Update(user);
                }

                dbContext.SaveChanges();
            }

            timer.Interval = TimeSpan.FromDays(30).TotalMilliseconds;
        }
    }
}

