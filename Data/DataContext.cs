using API.Models;
using System.Threading;
using System.Threading.Tasks;
using API.Identity.Entities;
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
  
  public DbSet<AppUser> User { get; set; }
  
}