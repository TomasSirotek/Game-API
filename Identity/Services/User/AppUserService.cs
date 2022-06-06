using System.Net;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using API.Dtos;
using API.ExternalServices;
using API.Identity.Entities;
using API.Repositories;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using API.Engines.Cryptography;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Rest;


namespace API.Services {
    
    public class AppUserService : IAppUserService {
        
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ICryptoEngine _cryptoEngine;
        
        public AppUserService (IUserRepository userRepository,IRoleRepository roleRepository,IEmailService emailService,IConfiguration configuration,ICryptoEngine cryptoEngine)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _configuration = configuration;
            _cryptoEngine = cryptoEngine;
            _roleRepository = roleRepository;

        }

        public async Task<List<AppUser>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }
        
        public async Task<AppUser> GetUserById(string id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task<AppUser> GetAsyncByEmail(string email)
        {
            return await _userRepository.GetAsyncByEmail(email);
        }
        
        public async Task<AppUser> RegisterUser(AppUser user,string password)
        {
            List<string> roles = new() {"User"};
            
            AppUser newUser = await CreateUser(user, roles, password);
            return newUser;
        }
        
        public async Task<AppUser> CreateUser(AppUser user,List<string> roles,string password)
        {
            List<AppRole> appRoles = new();
            List<AppRole> userRoles = appRoles;
            foreach (string role in roles)
            {
                var roleList = new AppRole
                {
                    Name = role
                };
                userRoles.Add(roleList);
            }
            user.Roles = userRoles;
            
            // hasPsw 
            var hashedPsw =  _cryptoEngine.Hash(password);
            user.PasswordHash = hashedPsw;
            
           //  validate EMAIL 
            if (user.Email != null)
            {
                // Create new user 
                AppUser createdUser = await _userRepository.CreateUser(user);
                    if (createdUser != null)
                    {
                        foreach (AppRole role in user.Roles)
                        {
                            AppRole fetchedRole = await _roleRepository.GetAsyncByName(role.Name);
                            if (fetchedRole != null) await _userRepository.AddToRoleAsync(createdUser,fetchedRole);
                            return null; // return could not return role for users
                        }
                            
                            // IF ACTIVE SEND EMAIL set active in email confirm function
                                if (user.IsActivated)
                                {
                                   //  await _userManager.GenerateEmailConfirmationTokenAsync(userFromDb);
                                    // Add Types of Emails as enums (OPTIONS FOR EMAIL) repair the url 
                                    var confirmEmailToken = "te";
                                    var link = $"https://localhost:5000/Authenticate/confirm?userId={user.Id}&token={confirmEmailToken}";
                                    _emailService.SendEmail(user.Email,user.FirstName,link,"Confirmation email");
                                    
                                }
                       // }
                        return createdUser;
                    }
            }
            return null;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            // AppUser user = await _userManager.FindByIdAsync(userId);
            // if (user == null) return IdentityResult.Failed();
            // var result = await _userManager.ConfirmEmailAsync(user, token);
            // if(result.Succeeded) return IdentityResult.Success;
            // return IdentityResult.Failed();
            return null;
        }
    }
}