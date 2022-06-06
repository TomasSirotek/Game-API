using API.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Services.Interfaces {
    public interface IAppUserService {
        Task<List<AppUser>> GetAllUsers();
        Task<AppUser> GetUserById(string id);

        Task<AppUser> GetAsyncByEmail(string email);

        Task<AppUser> RegisterUser(AppUser user, string password);
        Task<AppUser> CreateUser(AppUser user,List<string> roles,string password);

        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);


    }
}