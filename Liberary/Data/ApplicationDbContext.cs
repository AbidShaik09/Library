using Liberary.Models;
using Microsoft.EntityFrameworkCore;

namespace Liberary.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options ): base(options)
        {
                    
        }
        
        public DbSet<Liberaries> Books { get; set; }
    }
}
