using FinanceApp.Entities.Concrete;

namespace FinanceApp.MVC.Models.DTOs
{
    public class SaverDTO
    {
        public string AppUserId { get; set; }
        public Tip? SaverType { get; set; }
        public double SaverAmount { get; set; }
        public bool Status { get; set; }
        public double? TargetGoal { get; set; }
        public DateTime? TargetDate { get; set; }
        public Tip? Type { get; set; }
        public bool TargetStatus { get; set; }

    }
}
