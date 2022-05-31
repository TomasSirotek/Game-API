using API.Dtos;
using API.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.RepoInterface {
	public interface IUserRepository {

		Task<List<AppUser>> GetAllUsers();

		Task<AppUser> GetUserById(string id);
		
		Task<AppUser> CreateUser(AppUser user,string password);
		
		// Task<AppUser> UpdateUser(UserPutBindingModel model);
		
		Task<bool> DeleteUser(string id);
	}
}

