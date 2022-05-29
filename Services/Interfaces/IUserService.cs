using API.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Services.Interfaces {
    public interface IUserService {
        Task<List<AppUser>> GetAllUsers();
        Task<AppUser> GetUserById(string id);

        Task<IdentityResult> CreateUser(AppUser user,string password);

    }
}