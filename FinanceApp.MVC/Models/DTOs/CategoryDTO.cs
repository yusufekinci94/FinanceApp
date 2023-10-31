using FinanceApp.Entities.Concrete;

namespace FinanceApp.MVC.Models.DTOs
{
    public class CategoryDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? User { get; set; }
        public ICollection<Entry>? Entries { get; set; }

    }
}
