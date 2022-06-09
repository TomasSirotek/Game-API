using API.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Identity.Services.User {
    public interface IAppUserService {
        Task<List<AppUser>> GetAsync();
        Task<AppUser> GetAsyncById(string id);

        Task<AppUser> GetAsyncByEmail(string email);

        Task<AppUser> RegisterAsync(AppUser user, string password);
        Task<AppUser> CreateAsync(AppUser user,List<string> roles,string password);
        
        Task<AppUser> UpdateAsync(AppUser user,List<string> roles);
        
        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);

        Task<bool> DeleteAsync(string id);

        Task<bool> ChangePasswordAsync(AppUser user,string newPassword);
        
    }
}