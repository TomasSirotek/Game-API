using System;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos {
	public class UserDto {
		[Required]
		public string UserName { get; set; } = String.Empty;
		[Required]
		public string EmailAddress { get; set; } = String.Empty;
		[Required]
		public string Password { get; set; } = String.Empty;

	}
}

