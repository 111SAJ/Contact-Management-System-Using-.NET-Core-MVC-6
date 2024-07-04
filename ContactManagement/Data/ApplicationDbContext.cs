using ContactManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        {
        
        }

        //Model
        public DbSet<Contact> Contact { get; set; }
    }
}
