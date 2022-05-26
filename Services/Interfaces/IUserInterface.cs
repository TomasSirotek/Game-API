using API.Identity.Entities;

namespace API.Services.Interfaces {
    public interface IUserInterface {
        Task<List<AppUser>> GetAllUsers();
        Task<AppUser> GetUserById(string id);

    }
}