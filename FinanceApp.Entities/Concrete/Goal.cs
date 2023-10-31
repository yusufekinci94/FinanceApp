using FinanceApp.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Entities.Concrete
{
    public class Goal:BaseEntity
    {
        public string AppUserId { get; set; }
        public double? TargetGoal { get; set; }
        public DateTime? TargetDate { get; set; }
        public Tip? Type { get; set; }
        public bool TargetStatus { get; set; }
    }
}
