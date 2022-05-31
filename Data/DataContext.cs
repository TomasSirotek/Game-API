using API.Models;
using System.Threading;
using System.Threading.Tasks;
using API.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace API.Data {
  public class DataContext: IdentityDbContext<AppUser> {
      
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    
     // public DbSet<AppUser> user { get; set;}
     //
     // public DbSet<AppRole> role { get; set;}

    
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
        base.OnConfiguring(optionsBuilder);
    }
    
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.HasDefaultSchema("dbo");
    //
    //     modelBuilder.Entity<AppUser>(entity =>
    //     {
    //         entity.ToTable("user");
    //         entity.Property(i => i.Id).HasColumnName("Id").UseIdentityColumn();
    //         entity.Property(i => i.UserName).HasColumnName("UserName").UseIdentityColumn();
    //         entity.Property(i => i.Email).HasColumnName("Email").UseIdentityColumn();
    //         entity.Property(i => i.IsActive).HasColumnName("IsActive").UseIdentityColumn();
    //     });
    //
    //     base.OnModelCreating(modelBuilder);
    // }
    
  }
}