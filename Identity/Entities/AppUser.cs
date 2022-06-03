using API.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Identity.Entities; 

public class AppUser : IdentityUser {
    
    public string? Token { get; set; }
    
    public Address Address { get; set; }
    
    public List<AppRole> Roles { get; set; }
    
    public bool IsActive { get; set; }
   
    
    public AppUser() { }
    
    
}