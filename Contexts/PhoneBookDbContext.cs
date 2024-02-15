using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PhoneBookApp.Models;

namespace PhoneBookApp.Contexts
{
    public class PhoneBookDbContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        private string? _connectionString { get; set; }

        public PhoneBookDbContext()
        {
            var config = new ConfigurationBuilder()
                //.SetBasePath(Directory.GetCurrentDirectory())           // Modifies path from output dir to base project dir
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            _connectionString = config.GetConnectionString("ContactDatabase");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
