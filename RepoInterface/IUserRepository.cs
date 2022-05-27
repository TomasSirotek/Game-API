using API.Identity.Entities;

namespace API.RepoInterface {
	public interface IUserRepository {

		public Task<List<AppUser>> GetAllUsers();

		public Task<AppUser> GetUserById(string id);

		public Task<bool> DeleteUser(string id);
	}
}

