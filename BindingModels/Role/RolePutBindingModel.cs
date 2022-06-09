using System.ComponentModel.DataAnnotations;

namespace API.BindingModels.Role; 

public class RolePutBindingModel {
    [Required]
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
}