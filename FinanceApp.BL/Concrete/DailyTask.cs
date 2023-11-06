using FinanceApp.BL.Abstract;
using FinanceApp.DAL.Context;
using FinanceApp.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Timers;

namespace FinanceApp.BL.Concrete
{
    public class DailyTask : IDailyTask
    {
        private readonly System.Timers.Timer _timer;
        private readonly SqlDbContext _dbContext;

        public DailyTask()
        {
            _timer = new System.Timers.Timer();
            _dbContext = new SqlDbContext();

            //var now = DateTime.Now;
            //var nextDay = now.AddDays(1).Date.AddHours(0).AddMinutes(0).AddSeconds(0);
            //var interval = (nextDay - now).TotalMilliseconds;
            var now = DateTime.Now;
            var desiredTime = now.Date.AddDays(1).AddHours(0).AddMinutes(0).AddSeconds(0);
            var interval = (desiredTime - now).TotalMilliseconds;


            _timer.Interval = interval;
            _timer.Elapsed += ExecuteDailyTask;
        }

        public void Start()
        {
            
            _timer.Start();
        }

        private void ExecuteDailyTask(object sender, ElapsedEventArgs e)
        {
            
            var currentDate = DateTime.Now.Date;
            var monthlyEntities = _dbContext.Monthlies.ToList();
            var userEntities = _dbContext.Users.ToList();
            foreach(var userEntity in userEntities)
            {
                if ((userEntity.CreditPayDay.Value.Day == currentDate.Day)&&(userEntity.DeadLineExecute==true))
                {
                    userEntity.CreditDebt = userEntity.CreditDebt * userEntity.CreditCardInterest;
                    _dbContext.Users.Update(userEntity);
                    _dbContext.SaveChanges();
                }
            }
            foreach (var monthly in monthlyEntities)
            {
                if (monthly.PaymentDay == currentDate.Day&&monthly.Installment>0&&monthly.Status==true)
                {
                    monthly.Installment = monthly.Installment - 1;
                    _dbContext.Monthlies.Update(monthly);
                    _dbContext.SaveChanges();
                    Entry entry = new Entry()
                    {
                        Amount = monthly.Amount,
                        AppUserId = monthly.AppUserId,
                        Type = monthly.Type,
                        TypeMoney = monthly.TypeMoney,
                        CategoryIds = monthly.Categories,
                        Description =monthly.Name+ " Monthly Event",

                    };
                    _dbContext.Entries.Add(entry);
                    _dbContext.SaveChanges();
                    var user = _dbContext.Users.FirstOrDefault(x=>x.Id==entry.AppUserId);
                    if (user != null)
                    {
                        if (entry.Type == Tip.Giris)
                        {
                            user.TotalIncome += entry.Amount;
                            if (entry.TypeMoney == TipPara.Cash)
                            {
                                user.Cash += entry.Amount;
                            }
                            else if (entry.TypeMoney == TipPara.Credit)
                            {
                                user.CreditDebt -= entry.Amount;
                            }
                        }
                        else if (entry.Type == Tip.Cikis)
                        {
                            user.TotalOutgoing += entry.Amount;
                            if (entry.TypeMoney == TipPara.Cash)
                            {
                                user.Cash -= entry.Amount;
                            }
                            else if (entry.TypeMoney == TipPara.Credit)
                            {
                                user.CreditDebt += entry.Amount;
                            }
                        }

                        user.Balance = user.Cash - user.CreditDebt;



                        _dbContext.SaveChanges();
                    }
                }
            }

            _timer.Start();

        }
    }
}