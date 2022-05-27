using API.Identity.Entities;

namespace API.Services.Interfaces {
    public interface IUserService {
        Task<List<AppUser>> GetAllUsers();
        Task<AppUser> GetUserById(string id);

    }
}