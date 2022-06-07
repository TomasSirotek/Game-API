using System.ComponentModel.DataAnnotations;
using API.Models;
using MessagePack;

namespace API.Identity.Entities; 

public class AppUser  {
    
      public string Id { get; set; }
      
      public string Email { get; set; }
      
      public string UserName { get; set; }
      
      public string FirstName {get; set;}
      
      public string LastName { get; set; }
    
      public string PasswordHash { get; set; }
    
      public string Token { get; set; }
    
    // public Address Address { get; set; }
    
      public List<AppRole> Roles { get; set; }
    
      public bool IsActivated { get; set; }
      
      public DateTime CreatedAt { get; set; }
      
      public DateTime UpdatedAt { get; set; }
    
    
    public AppUser(string id,string email, string userName,string firstName,string lastName,bool isActivated)
    
    {
       Id = id;
       Email = email;
       UserName = userName;
       FirstName = firstName;
       LastName = lastName;
       IsActivated = isActivated;
    }
    
    public AppUser() { }
    
    
}