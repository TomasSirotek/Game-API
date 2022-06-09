using System.ComponentModel.DataAnnotations;

namespace API.BindingModels.User; 

public class UserPutBindingModel {
    [Required]
    public string Id { get; set; }
        
    [Required]
    public string UserName { get; set; }
        
    public string FirstName { get; set; }
        
    public string LastName { get; set; }
        
    [Required]
    [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
    public string Email { get; set; }

}