using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using API.Dtos;
using API.Identity.Entities;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;


namespace API.Controllers;

public class AuthController : DefaultController {

	private readonly IConfiguration _config;
	private readonly UserManager<AppUser> _userManager;

	public AuthController (IConfiguration config,UserManager<AppUser> userManager)
	{
		_config = config;
		_userManager = userManager;
	}

	[HttpPost ("Authenticate")] 
	public async Task<ActionResult<string>> Authenticate ([FromBody]AuthPostBindingModel request)
	{
		AppUser user = await _userManager.FindByEmailAsync(request.Email);
		if (user != null)
		{
			bool result = await _userManager.CheckPasswordAsync(user, request.Password);
			if(result){
				string token = CreateToken(user);
				user.Token = token;
				return Ok(user);
		}
			
		}
		// Make better error handeling 
		return NotFound("404");
		
	}

	#region POST
	[HttpPost ("Register")] 
	public async Task<ActionResult<AppUser>> Register (AuthPostBindingModel request)
	{
		var user = new AppUser()
		{
			UserName = request.Email,
			Email = request.Email
		};

		try
		{
			var result = await _userManager.CreateAsync(user, request.Password);
			return Ok(result);
		}
		catch (Exception ex)
		{
			throw ex;
		}
		
		

	}
	#endregion
	private string CreateToken(AppUser user)
	{
		List<Claim> claims = new () {
			new Claim(ClaimTypes.Email, user.Email),
			new Claim (ClaimTypes.Role, "Admin")
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

	// passoword hashing
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