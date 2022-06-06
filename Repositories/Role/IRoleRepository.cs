using API.Identity.Entities;

namespace API.Repositories {
    public interface IRoleRepository {
        
        Task<List<AppRole>> GetAsync();
        Task<AppRole> GetAsyncByName(string name);
        Task<AppRole> GetByIdAsync(string id);

        Task<bool> CreateAsync(AppRole user);
        //
        // //  Task<AppRole> UpdateAsync(AppRole role);
        //
        //  Task<bool> DeleteAsync(string id);

    }
}