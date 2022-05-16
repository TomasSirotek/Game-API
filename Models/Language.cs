using System.ComponentModel.DataAnnotations;

namespace API.Models {
    public class Language {
        [Required]
        public int Language_id { get; set; }
        public string Name { get; set; }
        
        public Language(int id,string name) {
            Language_id = id;
            Name = name;
        }
        public Language(){}
    }
    
   
}