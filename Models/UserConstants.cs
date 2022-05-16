using System;
namespace API.Models
{
	public class UserConstants
	{
		public static List<User> Users = new List<User>
		{
			new User() {
				UserName = "admin_",
				EmailAddress = "gmail@yahoo.com",
				Password = "123456",
				Role = "Admin" },
			new User() {
				UserName = "app_user",
				EmailAddress = "user@gmail.com",
				Password = "1234",
				Role = "User" }
		};
	}
}

