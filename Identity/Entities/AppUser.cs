using API.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Identity.Entities; 

public class AppUser : IdentityUser {
    
    //  public string Id { get; set; }
  
    // public override UserName {get; set;}
  
    // public string Email { get; set; }
    
   // public string Password { get; set; }
    
    public string? Token { get; set; }
    
    public List<IdentityRole> Roles { get; set; }
    
     public bool IsActive { get; set; }
   
    
    public AppUser(bool isActive)
    {
      //  Id = id;
     //   UserName = userName;
      //  Email = email;
      IsActive = isActive;
    }

    public AppUser() { }
    
    
}