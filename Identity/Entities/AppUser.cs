using API.Models;

namespace API.Identity.Entities; 

public class AppUser {
    public string Id { get; set; }
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public Address Address { get; set; }
    
    public string Token { get; set; }
    
    public string ProfilePictureURL { get; set; }

    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    
    public List<AppRole> RolesList { get; set; }

    public string CreatedAt { get; set; }
    
    public bool IsActive { get; set; }

    public AppUser(string id, string firstName,string lastName,string email,bool isActive) {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        IsActive = isActive;
    }

    public AppUser() { }
}