using System.ComponentModel.DataAnnotations;

namespace API.BindingModels.Authorization; 

public class RegisterPostBindingModel {
    [Required]
    public string UserName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long", MinimumLength = 4)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
}