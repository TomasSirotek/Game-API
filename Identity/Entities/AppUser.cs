using API.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Identity.Entities; 

public class AppUser : IdentityUser {

    public Address Address { get; set; }
    public string? Token { get; set; }

    public List<AppRole> Roles { get; set; }

    public AppUser(string token,bool isActive) {
        Token = token;
    }

    public AppUser() { }
}