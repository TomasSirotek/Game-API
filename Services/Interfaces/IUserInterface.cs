using API.Identity.Entities;

namespace API.Services.Interfaces {
    public interface IUserInterface {
        Task<AppUser> GetAllUsers();
    }
}