using API.Identity.Entities;
using API.RepoInterface;
using API.Services.Interfaces;


namespace API.Services {
    
    public class UserService : IUserInterface {
        
        private readonly IUserRepository _userRepository;

        public UserService (IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<AppUser>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }
        
        public async Task<AppUser> GetUserById(string id)
        {
            return await _userRepository.GetUserById(id);
        }

       
    }
}