using Microsoft.EntityFrameworkCore;

namespace Bitly.Model
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }


        public DbSet<Link> Links { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Link>().HasIndex(i => i.ShortLink).IsUnique();            
        }
    }
}