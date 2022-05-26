using API.Identity.Entities;

namespace API.RepoInterface {
	public interface IUserRepository {

		public Task<List<AppUser>> GetAllUsers();

		public Task<AppUser> GetUserById(string id);
	}
}

