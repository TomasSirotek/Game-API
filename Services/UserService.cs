using System.Net;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using API.Dtos;
using API.ExternalServices;
using API.Identity.Entities;
using API.RepoInterface;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Rest;


namespace API.Services {
    
    public class UserService :IUserService {
        
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailService _emailService;

        public UserService (IUserRepository userRepository,UserManager<AppUser> userManager,RoleManager<AppRole> roleManager,IEmailService emailService,IConfiguration configuration)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _configuration = configuration;

        }

        public async Task<List<AppUser>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }
        
        public async Task<AppUser> GetUserById(string id)
        {
            return await _userRepository.GetUserById(id);
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
                var test = new AppRole
                {
                    Name = role
                };
                userRoles.Add(test);
            }
            user.Roles = userRoles;
            
            // Create new user 
            if (user.Email != null)
            {
                IdentityResult createUser = await _userManager.CreateAsync(user,password);
                    if (createUser.Succeeded)
                    {
                     IdentityResult userCreated;
                        
                        // fetch new user
                        AppUser newUser = await _userManager.FindByIdAsync(user.Id);
                        // find app role by name 
                        
                        foreach (AppRole role in user.Roles)
                        {
                            AppRole roleExists = await _roleManager.FindByNameAsync(role.Name);
                            if (roleExists != null)
                                userCreated = await _userManager.AddToRoleAsync(newUser, role.Name);
                        }
                        
                        if (newUser != null)  // send email if is active yes ( for now false )
                        {
                            // Add Types of Emails as enums (OPTIONS FOR EMAIL) repair the url 
                            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                            var link = $"https://localhost:5000/Authenticate/confirm?userId={user.Id}&token={confirmEmailToken}";
                            _emailService.SendEmail(user.Email,user.UserName,link,"Confirmation email");
                        }
                        return newUser;
                    }
            }
            // var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
              //  var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);
              return null;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed();
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if(result.Succeeded) return IdentityResult.Success;
            return IdentityResult.Failed();
        }
        

       
    }
}