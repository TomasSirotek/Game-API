using API.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Identity.Entities; 

public class AppUser : IdentityUser {
    
      // public string Id { get; set; }
      //
      // public string UserName {get; set;}
      //
      // public string Email { get; set; }
    
   // public string Password { get; set; }
    
    public string? Token { get; set; }
    
    public Address Address { get; set; }
    
    public List<AppRole> Roles { get; set; }
    
     public bool IsActive { get; set; }
   
    
    // public AppUser()
    // {
    //    Id = id;
    //    UserName = userName;
    //   Email = email;
    //  // IsActive = isActive;
    // }

    public AppUser() { }
    
    
}