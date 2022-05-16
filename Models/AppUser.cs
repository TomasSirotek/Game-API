namespace API.Models {
    public class AppUser {
        public int User_id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }

        public AppUser(int id, string name, string email, int phoneNumber)
        {
            User_id = id;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;

        }
        public AppUser(){}
    }
}

