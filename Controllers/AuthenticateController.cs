using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using API.Configuration;
using API.Dtos;
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
	private readonly UserManager<AppUser> _userManager;
	private readonly IUserManager _userService;


	public AuthenticateController (IConfiguration configuration,UserManager<AppUser> userManager,IJWToken token, IUserManager userService)
	{
		_configuration = configuration;
		_userManager = userManager;
		_token = token;
		_userService= userService;
	}

	[HttpPost ()] 
	public async Task<ActionResult<string>> Authenticate ([FromBody]AuthPostBindingModel request)
	{
		AppUser user = await _userManager.FindByEmailAsync(request.Email);
		if (user != null)
		{
			// this block work when i define the roles cuz it creates it from role the token
			List<string> roles = new()
			{
				"Admin", "User"
			};

			List<AppRole> appRoles = new();
			List<AppRole> userRoles = appRoles;
			foreach (string role in roles)
			{
				var test = new IdentityRole
				{
					Name = role
				};
				//userRoles.Add(test);
			}
			// user.Roles = roles;
			
			bool result = await _userManager.CheckPasswordAsync(user, request.Password);
			if(result)
			{
				string token = _token.CreateToken(user.Roles.Select(role => role.Name).ToList(), user.Id, 24);
				user.Token = token;
				return Ok(user);
				// add email notificitation with date of loggin in DateTime.Now
			}
		}
		// Make better error handeling 
		return NotFound("404");
		
	}
	
	
	[HttpPost ("register")] 
	public async Task<IActionResult> Register ([FromBody]RegisterPostBindingModel request)
	{
		// AppUser user = new(
		// 	Guid.NewGuid().ToString(), 
		// 	request.Email,
		// 	false,
		// 	request.UserName
		// );

		AppUser user = new AppUser()
		{
			UserName = request.UserName,
			Email = request.Email
		};
		AppUser newUser = await _userService.RegisterUser(user, request.Password);
		if (newUser != null) return Ok(newUser);
	
		return BadRequest($"Could not register :(");
		
	}
	// this end-point works and confirms the email
	[HttpPost ("confirm")] 
	public async Task<IActionResult> ConfirmEmail (string userId,string token)
	{
		if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
			return NotFound();
		
		var result = await _userService.ConfirmEmailAsync(userId, token);
		if (result.Succeeded) return Ok("Email confirmed");
		 
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