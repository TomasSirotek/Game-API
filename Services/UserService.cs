using System.Security.Principal;
using API.Dtos;
using API.Identity.Entities;
using API.RepoInterface;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;


namespace API.Services {
    
    public class UserService : IUserService {
        
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
        
        public async Task<IdentityResult> CreateUser(UserPostBindingModel model)
        {
            // Image Upload 
            
            // Send new User 
            // IdentityResult userCreationResult = await _userRepository.CreateUser(user,model.Password,model.Roles);
            //
            //
            // IdentityResult userCreationResult = await _userRepository.CreateUser(user);
            // if (userCreationResult.Succeeded) {
            //     bool finalResult = false;
            //
            //     foreach (AppRole role in user.Roles) {
            //         // Validate if Role exists
            //         AppRole validatedRole = await _roleRepository.FindByNameAsync(role.Name, cancellationToken);
            //         if (validatedRole != null) {
            //             finalResult = await _roleRepository.AddRoleToUser(user, validatedRole);
            //
            //             if (!finalResult) {
            //                 return IdentityResult.Failed(new IdentityError { Description = $"Could not save role to user" });
            //             }
            //         } else {
            //             return IdentityResult.Failed(new IdentityError { Description = $"Could not find user role" });
            //         }
            //     }
            //
            //     if (finalResult) {
            //         return IdentityResult.Success;
            //     }
            
            //  await _userRepository.CreateUser();
            
            // assign roles 
            
            
            return null;
        }

       
    }
}