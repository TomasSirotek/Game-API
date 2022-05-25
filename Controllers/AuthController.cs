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


namespace API.Controllers;

public class AuthController : DefaultController {

	private readonly IConfiguration _config;
	private static readonly AppUser appUser = new ();

	public AuthController (IConfiguration config)
	{
		_config = config;
	}

	[HttpPost ("Authenticate")] 
	public async Task<ActionResult<string>> Authenticate ([FromBody]AuthPostBindingModel request)
	{
		if (appUser.UserName != request.UserName) return BadRequest ("User does not exist");

		if (!VerifyPasswordHash (request.Password, appUser.PasswordHash, appUser.PasswordSalt)) return BadRequest ("Wrong Password");
		
		string token = CreateToken(appUser);
		appUser.Token = token;
		return Ok (appUser);

	}

	// Register 
	[HttpPost ("Register")] 
	public async Task<ActionResult<AppUser>> Register ([FromBody] AuthPostBindingModel request)
	{
		CreatePasswordHash (request.Password, out byte [] passwordHash, out byte [] passwordSalt);
			
		appUser.UserName = request.UserName;
		appUser.PasswordHash = passwordHash;
		appUser.PasswordSalt = passwordSalt;

		return Ok (appUser);

	}

	private string CreateToken(AppUser user)
	{
		List<Claim> claims = new () {
			new Claim(ClaimTypes.Name, user.UserName),
			new Claim (ClaimTypes.Role, "User")
		};

		var key = new SymmetricSecurityKey (System.Text.Encoding.UTF8.GetBytes(
			_config.GetSection("JwtConfig:Secret").Value));

		var credentials = new SigningCredentials (key, SecurityAlgorithms.HmacSha512Signature);

		var token = new JwtSecurityToken (
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