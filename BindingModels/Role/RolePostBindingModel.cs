using System.ComponentModel.DataAnnotations;

namespace API.BindingModels.Role; 

public class RolePostBindingModel {
    [Required]
    public string Name { get; set; }
}