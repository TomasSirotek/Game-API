using System;
using System.ComponentModel.DataAnnotations;
using API.Enums;

namespace API.Models {
	public class User {

		//public User (string userName, string emailAddress, string role)
		//{
		//	UserName = userName;
		//	EmailAddress = emailAddress;
		//	Role = role;
		//}
		[Required]
		public string UserName { get; set; } = String.Empty;
		[Required]
		public string EmailAddress { get; set; } = String.Empty;
		public string Password { get; set; } = String.Empty;
		public byte [] PasswordHash { get; set; }
		public byte [] PasswordSalt { get; set; }
		public string Role { get; set; }
	}

}

