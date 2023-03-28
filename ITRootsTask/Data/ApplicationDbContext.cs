using Microsoft.EntityFrameworkCore;
using ITRootsTask.Models;

namespace ITRootsTask.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserVerification> UserVerifications{ get; set; }
        public DbSet<Invoice> Invoices{ get; set; }


    }
}
