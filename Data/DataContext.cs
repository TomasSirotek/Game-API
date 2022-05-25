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

  public DbSet<AppUser> Users { get; set;}
   
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.UseSerialColumns();
  }
  
}