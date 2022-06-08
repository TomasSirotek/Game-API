using System.ComponentModel.DataAnnotations;

namespace API.BindingModels.Authorization; 

public class AuthPostBindingModel {
    
    [Required]
    [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long", MinimumLength = 4)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
}