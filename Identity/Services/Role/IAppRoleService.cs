using API.Identity.Entities;

namespace API.Identity.Services.Role {
    public interface IAppRoleService {
        Task<List<AppRole>> GetAsync();
        
        Task<AppRole> GetAsyncById(string id);

        Task<AppRole> GetAsyncByName(string name);

        Task<AppRole> CreateAsync(AppRole role);

       // Task<AppRole> UpdateRole(AppRole role);
    }
}