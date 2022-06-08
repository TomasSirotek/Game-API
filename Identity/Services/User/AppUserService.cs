using API.Configuration;
using API.Engines.Cryptography;
using API.Enums;
using API.ExternalServices.Email;
using API.Identity.Entities;
using API.Repositories;
using Microsoft.AspNetCore.Identity;

namespace API.Identity.Services.User {
    
    public class AppUserService : IAppUserService {
        
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ICryptoEngine _cryptoEngine;
        private readonly IJWToken _token;
        
        public AppUserService (IUserRepository userRepository,IRoleRepository roleRepository,IEmailService emailService,IConfiguration configuration,ICryptoEngine cryptoEngine,IJWToken token)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _configuration = configuration;
            _cryptoEngine = cryptoEngine;
            _roleRepository = roleRepository;
            _token = token;

        }

        public async Task<List<AppUser>> GetAsync()
        {
            return await _userRepository.GetAllUsers();
        }
        
        public async Task<AppUser> GetAsyncById(string id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task<AppUser> GetAsyncByEmail(string email)
        {
            return await _userRepository.GetAsyncByEmail(email);
        }
        
        public async Task<AppUser> RegisterAsync(AppUser user,string password)
        {
            List<string> roles = new() {"User"};
            
            AppUser newUser = await CreateAsync(user, roles, password);
            return newUser;
        }
        
        public async Task<AppUser> CreateAsync(AppUser user,List<string> roles,string password)
        {
            List<AppRole> appRoles = new();
            List<AppRole> userRoles = appRoles;
            foreach (var role in roles)
            {
                var roleList = new AppRole
                {
                    Name = role
                };
                userRoles.Add(roleList);
            }
            user.Roles = userRoles;
            
            var hashedPsw =  _cryptoEngine.Hash(password);
            user.PasswordHash = hashedPsw;
            
            AppUser createdUser = await _userRepository.CreateUser(user);
            if (createdUser == null)
                    throw new Exception("Could now create user");
            foreach (AppRole role in user.Roles)
            {
                AppRole fetchedRole = await _roleRepository.GetAsyncByName(role.Name);
                        if (fetchedRole == null) 
                            throw new Exception("Could now create roles for user");
                        await _userRepository.AddToRoleAsync(createdUser, fetchedRole);
            }
            if (user is {IsActivated: false})
            {
                var confirmEmailToken = _token.CreateToken(user.Roles.Select(role => role.Name).ToList(), user.Id, 24);;
                var link = $"https://localhost:5000/Authenticate/confirm?userId={user.Id}&token={confirmEmailToken}";
                _emailService.SendEmail(user.Email,user.UserName,link,"Confirm email");
            }
            AppUser fetchedNewUser = await _userRepository.GetUserById(createdUser.Id);
            return fetchedNewUser;
        }
            
            //  await _userManager.GenerateEmailConfirmationTokenAsync(userFromDb);
            // Add Types of Emails as enums (OPTIONS FOR EMAIL) repair the url 
            
            public async Task<AppUser> UpdateAsync(AppUser user)
            {
                AppUser updatedUser = await _userRepository.UpdateAsync(user);
                if (updatedUser == null) throw new Exception("Could not update user ");
                updatedUser.Roles.Clear();
                
                foreach (AppRole role in user.Roles)
                {
                    AppRole fetchedRole = await _roleRepository.GetAsyncByName(role.Name);
                    if(fetchedRole == null)  throw new Exception($"Could not find role with name {role.Name}");
                    await _userRepository.AddToRoleAsync(updatedUser, fetchedRole);
                }
                
                return updatedUser;
            }
            
        public async Task<bool> ChangePasswordAsync(AppUser user, string newPassword)
        {
            var hashedPsw =  _cryptoEngine.Hash(newPassword);
            return await _userRepository.ChangePasswordAsync(user,hashedPsw);
        }
        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
             AppUser user = await _userRepository.GetUserById(userId);
             // TODO: verify token => set user to IsActivated(true) and send email bout confirm-success
            var activated =  await SetEmailConfirmedAsync(user, true);
             //  var result = await _userManager.ConfirmEmailAsync(user, token);
              if(!activated)  throw new Exception($"Could not confirm user with email {user.Email}");;
              return null;
        }

        public async Task<bool> SetEmailConfirmedAsync(AppUser user, bool confirmed)
        {
            return await _userRepository.SetActiveAsync(user.Id, confirmed);
        }
        public async Task<bool> DeleteAsync(string id)
        {
            return await _userRepository.DeleteUser(id);
        }
    }
}
