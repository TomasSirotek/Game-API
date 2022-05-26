using API.Identity.Entities;
using API.RepoInterface;

namespace API.Repositories; 

public class UserRepository : IUserRepository {

	private readonly List<AppUser> _appUsers;

		public UserRepository() 
		{
			_appUsers = new () {
 				new AppUser(
	                "343",
	                "Admin",
	                "email@yahoo.com",
	                true
	                ),
                new AppUser(
	                "222",
	                "User",
	                "email@yahoo.com",
	                false
                ),
                new AppUser(
	                "222",
	                "User",
	                "email@yahoo.com",
	                true
                )

			};
		}
	
		public async Task<List<AppUser>> GetAllUsers()
		{
			return _appUsers;
		}
		public async Task<AppUser> GetUserById(string id)
		{
			var user = _appUsers.FirstOrDefault(x => x.Id == id);
			return user;
		}
}