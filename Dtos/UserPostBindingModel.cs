namespace API.Dtos; 

public class UserPostBindingModel {
    
    public string Id { get; set; }
    
    public string UserName { get; set; }
    
    public string Email { get; set; }
    
    public byte[] Password { get; set; }
    
    public bool? isActive { get; set; }

}