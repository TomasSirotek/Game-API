using System.ComponentModel.DataAnnotations;

namespace API.Dtos; 

public class UserPostBindingModel {
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string UserName { get; set; }
    [Required]
    [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    
    public List<string> Roles { get; set; }

}