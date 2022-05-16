using API.Models;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace API.Data; 

public class DataContext: DbContext {
  
  public DataContext(DbContextOptions<DataContext> options) : 
    base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.UseSerialColumns();
  }
    
  public DbSet<Project> Project { get; set; }
  public DbSet<AppUser> User { get; set; }
  public DbSet<Category> Category { get; set; }
  public DbSet<Language> Language { get; set; }

  
}