using API.Identity.Entities;
using API.Repositories;

namespace API.Identity.Services.Role {
    
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
        
        public async Task<AppRole> GetAsyncByName(string name)
        {
            return await _roleRepository.GetAsyncByName(name);
        }
        public async Task<AppRole> CreateAsync(AppRole role)
        {
            bool result  = await _roleRepository.CreateAsync(role);
            if (!result)  throw new Exception("Role cannot be empty");
            AppRole fetchedRole = await _roleRepository.GetByIdAsync(role.Id);
            return fetchedRole;
        }
        
        public async Task<AppRole> UpdateAsync(AppRole role)
        {
            if (role == null) 
                throw new Exception("Role cannot be empty");
            // try to update
            AppRole roleUpdated = await _roleRepository.UpdateAsync(role);
            if (roleUpdated == null) 
                throw new Exception("Role could not be created");
            // fetch updated role
            AppRole updatedRole = await _roleRepository.GetByIdAsync(role.Id);
            // if done return back 
            return updatedRole;
        }
        
        public async Task<bool> DeleteAsync(string id)
        {
            return await _roleRepository.DeleteAsync(id);
        }
        
    }
}