using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Sms.Models;

namespace Sms.Models.Context
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=(localdb)\\MSSQLLocalDB;initial catalog=SmsDeneme;integrated security=true;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Sms.Models.Client> Client { get; set; } = default!;
    }
}
