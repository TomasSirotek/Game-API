using API.Dtos;
using API.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Repositories; 

public interface IUserRepository {

	Task<List<AppUser>> GetAllUsers();

	Task<AppUser> GetUserById(string id);
		
	Task<AppUser> CreateUser(AppUser user,string password);

	Task<AppUser> AddToRoleAsync(AppUser user, AppRole role);
		
	// Task<AppUser> UpdateUser(UserPutBindingModel model);
		
	Task<bool> DeleteUser(string id);
}