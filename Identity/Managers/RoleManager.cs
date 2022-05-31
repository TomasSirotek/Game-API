using API.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Identity.Managers {
    
    public class RoleManager : IRoleManager {
        
        private readonly RoleManager<AppRole> _roleManager;

        public  RoleManager(RoleManager<AppRole> roleManager) {
            _roleManager = roleManager;
        }
        
        public async Task<List<AppRole>> GetAllRoles()
        {
            return  _roleManager.Roles.ToList();
        }
        
        public async Task<AppRole> GetRoleByName(string name)
        {
            return await _roleManager.FindByNameAsync(name);
        }
        
        public async Task<AppRole> GetRoleById(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }

        public async Task<AppRole> CreateRole(AppRole role)
        {
            if (role != null)
            {
            IdentityResult roleCreated = await _roleManager.CreateAsync(role);
            if (roleCreated.Succeeded)
            {
                AppRole newRole = await _roleManager.FindByIdAsync(role.Id); 
                return newRole;
            }
            }

            return null;
            
            // return exceptions
        }
        
        public async Task<AppRole> UpdateRole(AppRole role)
        {
            if (role != null)
            {
                AppRole updatedRole = await _roleManager.FindByIdAsync(role.Id); 
                IdentityResult updateRole = await _roleManager.UpdateAsync(updatedRole);
                if (updateRole.Succeeded) return updatedRole;
                
            }

            return null;
        }
        
    }
}