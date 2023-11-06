using FinanceApp.DAL.Context;
using FinanceApp.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.MVC.Models.DTOs
{
    public class ProgressDTO
    {
        
        public ICollection<Entry>? Entries { get; set; } 
        public ICollection<Entry>? RepeatingOutgoings { get; set; } 
        public ICollection<Monthly>? Monthlies { get; set; }
        public ICollection<Save>? Saves { get; set; }
        public AppUser? User { get; set; }
        public Goal? Goal { get; set; }
        public List<string?>? categories { get; set; }
    }

}
