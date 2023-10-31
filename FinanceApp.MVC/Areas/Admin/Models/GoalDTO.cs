using FinanceApp.Entities.Concrete;

namespace FinanceApp.MVC.Areas.Admin.Models
{
    public class GoalDTO
    {
        public string AppUserId { get; set; }
        public double? TargetGoal { get; set; }
        public DateTime? TargetDate { get; set; }
        public Tip? Type { get; set; }
        public bool TargetStatus { get; set; }
    }
}
