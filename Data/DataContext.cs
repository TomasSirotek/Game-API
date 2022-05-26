using API.Models;
using System.Threading;
using System.Threading.Tasks;
using API.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace API.Data {
  public class DataContext: IdentityDbContext<AppUser> {
      
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    
    public DbSet<AppUser> user { get; set;}
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("PostgresAppCon");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
    
  }
}