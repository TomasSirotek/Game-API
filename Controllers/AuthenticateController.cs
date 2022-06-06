using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using API.Configuration;
using API.Dtos;
using API.Engines.Cryptography;
using API.Identity.Entities;
using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;


namespace API.Controllers;

public class AuthenticateController : DefaultController {

	private readonly IConfiguration _configuration;
	private readonly IJWToken _token;
	private readonly IAppUserService _userService;
	private readonly ICryptoEngine _cryptoEngine;

	



	public AuthenticateController (IConfiguration configuration,IJWToken token,IAppUserService userService,ICryptoEngine cryptoEngine)
	{
		_configuration = configuration;
		_token = token;
		_userService = userService;
		_cryptoEngine = cryptoEngine;

	}

	[HttpPost ()] 
	public async Task<ActionResult<string>> Authenticate ([FromBody]AuthPostBindingModel request)
	{
		AppUser user = await _userService.GetAsyncByEmail(request.Email);
		if (user != null)
		{
			bool varifiedPsw = _cryptoEngine.HashCheck(user.PasswordHash, request.Password);
			if(varifiedPsw)
			{
				// fix role "name" to role.name
				 string token = _token.CreateToken(user.Roles.Select(role => "name").ToList(), user.Id, 24);
				 user.Token = token;
				 return Ok(user);
				// add email notificitation with date of loggin in DateTime.Now
			}
		}
		// Make better error handeling 
		return NotFound("404");
		
	}
	
	
	[HttpPost ("register")] 
	public async Task<IActionResult> Register ([FromBody]RegisterPostBindingModel model)
	{
		
		AppUser user = new AppUser()
		 {
			 Id = Guid.NewGuid().ToString(),
			 UserName = model.UserName,
			 Email = model.Email,
			 IsActivated = false,
			 CreatedAt = DateTime.Now,
			 UpdateAt = DateTime.Now
		 };
		AppUser newUser = await _userService.RegisterUser(user, model.Password);
		if (newUser != null) return Ok(newUser);
	
		return BadRequest($"Could not register :(");
		
	}
	// this end-point works and confirms the email
	[HttpPost ("confirm")] 
	public async Task<IActionResult> ConfirmEmail (string userId,string token)
	{
		if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
			return NotFound();
		
	//	var result = await _userService.ConfirmEmailAsync(userId, token);
	// 	if (result.Succeeded) return Ok("Email confirmed");
		 
		return BadRequest($"Could not confirm account for user with id {userId}");
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