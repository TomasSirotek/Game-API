namespace API.Identity.Entities; 

public class AppRole {
    public string Id { get; set; }
    public string Name { get; set; }
    
    public AppRole(string id, string name) {
        Id = id;
        Name = name;
    }

    public AppRole() { }
}