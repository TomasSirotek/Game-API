using API.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Services.Interfaces {
    public interface IUserService {
        Task<List<AppUser>> GetAllUsers();
        Task<AppUser> GetUserById(string id);

        Task<AppUser> RegisterUser(AppUser user, string password);
        Task<AppUser> CreateUser(AppUser user,List<string> roles,string password);

        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);


    }
}