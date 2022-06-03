using API.Models;
using System.Threading;
using System.Threading.Tasks;
using API.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace API.Data {
  public class DataContext: IdentityDbContext<AppUser> {
      
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    
  }
}