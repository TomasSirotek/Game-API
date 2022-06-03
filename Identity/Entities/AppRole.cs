using Microsoft.AspNetCore.Identity;

namespace API.Identity.Entities; 

public class AppRole : IdentityRole {
     //public string Id { get; set; }
   //  public string Name { get; set; }
    
    public AppRole(string name) {
       // Id = id;
       // Name = name;
    }

    public AppRole() { }
}