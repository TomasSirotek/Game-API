using API.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Identity.Entities; 

public class AppUser : IdentityUser {
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string? Token { get; set; }
    
    public AppUser(string firstName,string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
     
    }

    public AppUser() { }
}