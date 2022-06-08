using API.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Repositories; 

public interface IUserRepository {

	Task<List<AppUser>> GetAllUsers();

	Task<AppUser> GetUserById(string id);

	Task<AppUser> GetAsyncByEmail(string email);
		
	Task<AppUser> CreateUser(AppUser user);

	Task<AppUser> AddToRoleAsync(AppUser user, AppRole role);

	Task<bool> ChangePasswordAsync(AppUser user, string newPasswordHash);

	Task<AppUser> UpdateAsync(AppUser user);

	Task<bool> SetActiveAsync(string id, bool result);
	Task<bool> DeleteUser(string id);
}