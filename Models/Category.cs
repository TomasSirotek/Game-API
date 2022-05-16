namespace API.Models {
    public class Category {
        public int Category_id { get; set; }
        public string Name { get; set; }

        
        public Category(int id,string name) {
            Category_id = id;
            Name = name;
        }
        public Category(){}
    }
    
   
}