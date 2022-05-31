using API.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Identity.Managers {
    public interface IRoleManager {
        Task<List<AppRole>> GetAllRoles();
        
        Task<AppRole> GetRoleByName(string name);

        Task<AppRole> GetRoleById(string id);

        Task<AppRole> CreateRole(AppRole role);

        Task<AppRole> UpdateRole(AppRole role);
    }
}