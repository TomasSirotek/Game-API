namespace API.BindingModels.Authorization;
using System.ComponentModel.DataAnnotations;

public class ChangePasswordBindingModel {
    [Required]
    public string UserId { get; set; } 
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long", MinimumLength = 4)]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; } 
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long", MinimumLength = 4)]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } 
}