using API.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Identity.Services {
    public interface IAppRoleService {
        Task<List<AppRole>> GetAsync();
        
        Task<AppRole> GetAsyncById(string id);

        Task<AppRole> GetAsyncByName(string name);

        Task<AppRole> CreateAsync(AppRole role);

       // Task<AppRole> UpdateRole(AppRole role);
    }
}