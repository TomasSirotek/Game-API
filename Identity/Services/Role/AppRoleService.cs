using API.Identity.Entities;
using API.Repositories;
using Microsoft.AspNetCore.Identity;

namespace API.Identity.Services {
    
    public class AppRoleService : IAppRoleService {
        
        private readonly IRoleRepository _roleRepository;

        public  AppRoleService(IRoleRepository roleRepository) {
            _roleRepository = roleRepository;
        }
        
        public async Task<List<AppRole>> GetAsync()
        {
            return await _roleRepository.GetAsync();
        }
        
        public async Task<AppRole> GetAsyncById(string id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }

        public async Task<AppRole> CreateAsync(AppRole role)
        {
            bool result  = await _roleRepository.CreateAsync(role);
            if (result)
            {
                AppRole fetchedRole = await _roleRepository.GetByIdAsync(role.Id);
                return fetchedRole;
            }
            return null;
            // return exceptions
        }
        
        // public async Task<AppRole> UpdateRole(AppRole role)
        // {
        //     if (role != null)
        //     {
        //         // try to update
        //         IdentityResult roleUpdated = await _roleManager.UpdateAsync(role);
        //         // fetch updated role
        //         AppRole updatedRole = await _roleManager.FindByIdAsync(role.Id);
        //         // if done return back 
        //         if (roleUpdated.Succeeded) return updatedRole;
        //         
        //     }
        //
        //     return null;
        // }
        
    }
}