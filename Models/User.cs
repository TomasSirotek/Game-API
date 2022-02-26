using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models {
	public class User {
		[Required]
		public string UserName { get; set; } = String.Empty;
		[Required]
		public string EmailAddress { get; set; } = String.Empty;
		public byte [] PasswordHash { get; set; }
		public byte [] PasswordSalt { get; set; }
		//public string Password { get; set; }
		//public string EmailAdress { get; set; }
		//public string Role { get; set; }
	}
}

