using API.Identity.Entities;

namespace API.Repositories {
    public interface IRoleRepository {
        
        Task<List<AppRole>> GetAsync();
        Task<AppRole> GetByIdAsync(string id);
		
        Task<AppRole> CreateAsync(AppRole role);
       //
       // //  Task<AppRole> UpdateAsync(AppRole role);
       //
       //  Task<bool> DeleteAsync(string id);
    
    }
}