using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using API.Configuration;
using API.Dtos;
using API.Identity.Entities;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;


namespace API.Controllers;

public class AuthenticateController : DefaultController {

	private readonly IConfiguration _config;
	private readonly IJWToken _token;
	private readonly UserManager<AppUser> _userManager;

	public AuthenticateController (IConfiguration config,UserManager<AppUser> userManager,IJWToken token)
	{
		_config = config;
		_userManager = userManager;
		_token = token;
	}

	[HttpPost ()] 
	public async Task<ActionResult<string>> Authenticate ([FromBody]AuthPostBindingModel request)
	{
		AppUser user = await _userManager.FindByEmailAsync(request.Email);
		if (user != null)
		{
			bool result = await _userManager.CheckPasswordAsync(user, request.Password);
			if(result)
			{
				string token = _token.CreateToken(user);
				user.Token = token;
				return Ok(user);
			}
		}
		// Make better error handeling 
		return NotFound("404");
		
	}

	// private void CreatePasswordHash (string password, out byte [] passwordHash, out byte [] passwordSalt)
	// {
	// 	using var hmac = new HMACSHA512 ();
	// 	passwordSalt = hmac.Key;
	// 	passwordHash = hmac.ComputeHash (System.Text.Encoding.UTF8.GetBytes (password));
	// }
	// // Verification psw hash
	// private bool VerifyPasswordHash(string password,byte[] passwordHash, byte[] passwordSalt)
	// {
	// 	using var hmac = new HMACSHA512 (passwordSalt);
	// 	var computeHash = hmac.ComputeHash (System.Text.Encoding.UTF8.GetBytes (password));
	// 	return computeHash.SequenceEqual (passwordHash);
	// }

}