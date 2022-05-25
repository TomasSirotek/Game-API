using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using API.Dtos;
using API.Identity.Entities;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

namespace API.Controllers {
	[Route ("[controller]")]
	public class AuthController : ControllerBase {

		private readonly IConfiguration _config;
		private static readonly AppUser appUser = new ();

		public AuthController (IConfiguration config)
		{
			_config = config;
		}

		[HttpPost ("Authenticate")] 
		public async Task<ActionResult<string>> Authenticate ([FromBody]CreateUserDto request)
		{
			 if (appUser.Email != request.Email) return BadRequest ("User does not exist");

			 if (!VerifyPasswordHash (request.Password, appUser.PasswordHash, appUser.PasswordSalt)) return BadRequest ("Wrong Password");


			string token = CreateToken(appUser);
			return Ok (token);

		}

		// Register 
		[HttpPost ("Register")] // From body 
		public async Task<ActionResult<AppUser>> Register ([FromBody] CreateUserDto request)
		{
			CreatePasswordHash (request.Password, out byte [] passwordHash, out byte [] passwordSalt);
			
			appUser.Email = request.Email;
			appUser.PasswordHash = passwordHash;
			appUser.PasswordSalt = passwordSalt;

			return Ok (appUser);

		}


		private string CreateToken(AppUser user)
		{
			List<Claim> claims = new () {
				new Claim(ClaimTypes.Email, user.Email)
				// new Claim (ClaimTypes.Role, user.RolesList),
			};

			var key = new SymmetricSecurityKey (System.Text.Encoding.UTF8.GetBytes(
				_config.GetSection("Jwt:AuthToken").Value));

			var credentials = new SigningCredentials (key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken (
 				_config ["Jwt:Issuer"],
				_config["Jwt:Audience"],

				claims: claims,
				expires: DateTime.Now.AddMinutes(15),
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

