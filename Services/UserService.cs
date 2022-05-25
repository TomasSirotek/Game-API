using API.Identity.Entities;
using API.Services.Interfaces;

namespace API.Services {

    
    public class UserService : IUserInterface {

        public Task<AppUser> GetAllUsers()
        {
            AppUser user = new(
                Guid.NewGuid().ToString(),
                "Admin",
                "email@yahoo.com",
                false
                
            );
            return null;
        }
    }
}