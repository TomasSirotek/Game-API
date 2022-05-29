using API.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Identity.Entities; 

public class AppUser : IdentityUser {
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Email { get; set; }
    
   // public string Password { get; set; }
    
    public string? Token { get; set; }
    
    public List<AppRole> Roles { get; set; }
    
    public AppUser(string firstName,string lastName,string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;

    }

    public AppUser() { }
}