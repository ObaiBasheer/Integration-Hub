using Integration_Hub.Models;
using Microsoft.EntityFrameworkCore;

namespace Integration_Hub.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Integration> Integrations { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Mapping> Mappings { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define relationships and constraints here if needed
        }
    }

}
