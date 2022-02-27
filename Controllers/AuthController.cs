using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using API.Dtos;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers {
	[Route ("api/[controller]")]
	public class AuthController : ControllerBase {

		private readonly IConfiguration _config;
		private static readonly User user = new ();

		public AuthController (IConfiguration config)
		{
			_config = config;
		}

		[HttpPost ("Authenticate")] // From body 
		public async Task<ActionResult<string>> Authenticate ([FromBody]UserDto request)
		{
			if (user.UserName != request.UserName) return BadRequest ("User does not exist");

			if (user.EmailAddress != request.EmailAddress) return BadRequest ("Email Address does not exist");

			if (!VerifyPasswordHash (request.Password, user.PasswordHash, user.PasswordSalt)) return BadRequest ("Wrong Password");


			string token = CreateToken (user);
			return Ok (token);

		}

		// Register 
		[HttpPost ("Register")] // From body 
		public async Task<ActionResult<User>> Register ([FromBody] UserDto request)
		{
			CreatePasswordHash (request.Password, out byte [] passwordHash, out byte [] passwordSalt);

			user.UserName = request.UserName;
			user.EmailAddress = request.EmailAddress;
			user.PasswordHash = passwordHash;
			user.PasswordSalt = passwordSalt;

			return Ok (user);

		}


		private string CreateToken(User user)
		{
			List<Claim> claims = new () {
				new Claim(ClaimTypes.NameIdentifier, user.UserName),
				new Claim (ClaimTypes.Email, user.EmailAddress),
				new Claim (ClaimTypes.Role, user.Role),
			};

			var key = new SymmetricSecurityKey (System.Text.Encoding.UTF8.GetBytes(
				_config.GetSection("Jwt:AuthToken").Value));

			var credentials = new SigningCredentials (key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken (
 				_config ["Jwt:Issuer"],
				_config["Jwt:Audinece"],

				claims: claims,
				expires: DateTime.Now.AddMinutes (15),
				signingCredentials: credentials
				);
			var jwt = new JwtSecurityTokenHandler ().WriteToken (token);

			return jwt;
		}

		private void CreatePasswordHash (string password, out byte [] passwordHash, out byte [] passwordSalt)
		{
			using var hmac = new HMACSHA512 ();
			passwordSalt = hmac.Key;
			passwordHash = hmac.ComputeHash (System.Text.Encoding.UTF8.GetBytes (password));
		}
		// Verification psw hash
		private bool VerifyPasswordHash(string password,byte[] passwordHash, byte[] passwordSalt)
		{
			using var hmac = new HMACSHA512 (passwordSalt);
			var computeHash = hmac.ComputeHash (System.Text.Encoding.UTF8.GetBytes (password));
			return computeHash.SequenceEqual (passwordHash);
		}

	
	}
}

