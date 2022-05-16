using System.ComponentModel.DataAnnotations;

namespace API.Models {
    public class Project {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Client { get; set; }
        public string Host { get; set; }
        public string Image_url { get; set; }
        public DateTime Release_date { get; set; } 
        public List<AppUser> User { get; set; }
        public List<Category> Category { get; set; }
        public List<Language> Language { get; set; }
        
        
        public Project(int id, string title, string description, string client, string host,string image_url, DateTime release_date) {
            Id = id;
            Title = title;
            Description = description;
            Client = client;
            Host = host;
            Image_url = image_url;
            Release_date = release_date;
        }
        public Project(){}
    }
}