using API.BindingModels.Authorization;
using API.Configuration;
using API.Engines.Cryptography;
using API.ExternalServices.Email;
using API.Identity.Entities;
using API.Identity.Services.User;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;

namespace API.Controllers;

public class AuthenticateController : DefaultController {

	private readonly IEmailService _emailService;
	private readonly IJWToken _token;
	private readonly IAppUserService _userService;
	private readonly ICryptoEngine _cryptoEngine;
	public AuthenticateController (IEmailService emailService,IJWToken token,IAppUserService userService,ICryptoEngine cryptoEngine)
	{
		_emailService = emailService;
		_token = token;
		_userService = userService;
		_cryptoEngine = cryptoEngine;
	}

	[HttpPost ()] 
	public async Task<ActionResult> Authenticate ([FromBody]AuthPostBindingModel request)
	{
		AppUser user = await _userService.GetAsyncByEmail(request.Email);
		if (user == null)
			return BadRequest($"User email: {request.Email} is not correct!");

		if (!_cryptoEngine.HashCheck(user.PasswordHash, request.Password))
			return BadRequest("Password incorrect !");

		var token = _token.CreateToken(user.Roles.Select(role => role.Name).ToList(), user.Id, 24);
				 user.Token = token;
				 var body = $"Recent log in to account have been noticed ! Date {new Timestamp()}"; // move separately 
		if(!string.IsNullOrWhiteSpace(token))
			_emailService.SendEmail(user.Email,user.UserName,body,"Recent activity");
		return Ok(user);
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
			 CreatedAt = DateTime.Now
		 };
		AppUser newUser = await _userService.RegisterAsync(user, model.Password);
		if (newUser == null) return BadRequest($"Could not register ");
		return Ok(newUser);

	}
	// this end-point works and confirms the email
	[HttpPost ("confirm-email")] 
	public async Task<IActionResult> ConfirmEmail(string userId,string token)
	{
		if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
			return NotFound();
		
		var result = await _userService.ConfirmEmailAsync(userId, token);
	  	if (!result.Succeeded) 
		    return BadRequest($"Could not confirm account for user with id {userId}");

		return Ok("Email confirmed !");
	}
	
}