using Microsoft.EntityFrameworkCore;
using NetCoreAI.Project01_ApiDemo.Entities;

namespace NetCoreAI.Project01_ApiDemo.Context
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=NıTRO-AN515-57;initial catalog=ApiAIDb;integrated security=true;trustservercertificate=true");
        }
            public DbSet<Customer> Customers { get; set; }
    }
}
