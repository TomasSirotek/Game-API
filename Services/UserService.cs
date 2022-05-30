using System.Security.Principal;
using API.Dtos;
using API.ExternalServices;
using API.Identity.Entities;
using API.RepoInterface;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;


namespace API.Services {
    
    public class UserService : IUserService {
        
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public UserService (IUserRepository userRepository,UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager,IEmailService emailService)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;

        }

        public async Task<List<AppUser>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }
        
        public async Task<AppUser> GetUserById(string id)
        {
            return await _userRepository.GetUserById(id);
        }
        
        public async Task<IdentityResult> CreateUser(AppUser user,string password)
        {
            // Create new user 
            //IdentityResult createUser = await _userManager.CreateAsync(user,password);
            
            // Send him email
            // dependency injection of mail service 
           // if (createUser.Succeeded)
           // {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                _emailService.SendEmail("willard.orn27@ethereal.email",user.FirstName,token,"test");

            //}
            
            
            
            
            return IdentityResult.Success;
        }
        

       
    }
}