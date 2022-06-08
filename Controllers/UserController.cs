using API.BindingModels.Authorization;
using API.BindingModels.User;
using API.Engines.Cryptography;
using API.Enums;
using API.Helpers;
using API.Identity.Entities;
using API.Identity.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers; 

public class UserController : DefaultController
{
    private readonly IAppUserService _userService;
    private readonly ICryptoEngine _cryptoEngine;

    public UserController (IAppUserService userService,ICryptoEngine cryptoEngine)
    {
        _userService = userService;
        _cryptoEngine = cryptoEngine;

    }
    
    #region GET
    [HttpGet()]
    //[Authorize(Roles ="Admin")]
    [AllowAuthorized(AccessRoles.Admin)]
    //[AllowAnonymous]
    public async Task<IActionResult> GetAllAsync ()
    {
        List<AppUser> userList = await _userService.GetAsync();
        if (userList.IsNullOrEmpty())
            return BadRequest($"Could not find any users");
        return Ok(userList);
    }
    
    
    [HttpGet("{id}")]
    //[AllowAuthorizedAttribute(AccessRoles.Admin)]
    public async Task<IActionResult> GetAsyncById(string id)
    {
        AppUser user = await _userService.GetAsyncById(id);
        if (user != null) 
            return Ok (user);
        return BadRequest($"Could not find user with Id : {id}");
    }

    #endregion
    
    #region POST
    [HttpPost()]
    //[AllowAuthorizedAttribute(AccessRoles.Admin)]
    public async Task<IActionResult> CreateAsync([FromBody]UserPostBindingModel request)
    {
        // move to services 
        AppUser user = new AppUser()
       {
           Id = Guid.NewGuid().ToString(),
           Email = request.Email,
           FirstName = request.FirstName,
           LastName = request.LastName,
           IsActivated = request.isConfirmed
           
       };
        AppUser resultUser = await _userService.CreateAsync(user,request.Roles, request.Password);
        
        if(resultUser == null) 
            return BadRequest($"Could not create user with Email : {request.Email}");
        return Ok(resultUser);
    }
 
    
    #endregion
    
    #region PUT
    [HttpPut()]
    //[Authorize(Roles ="Admin")]
    public async Task<IActionResult> UpdateAsync([FromBody]UserPutBindingModel request)
    {
        // move to services 
        AppUser fetchedUser = await _userService.GetAsyncById(request.Id);
        if(fetchedUser == null) 
            return BadRequest($"Could not find user with Id : {request.Id}");
        
        AppUser requestUser = new AppUser()
        {
            UserName = request.UserName,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UpdatedAt = DateTime.Now
    
        };
        AppUser updatedUser = await _userService.UpdateAsync(requestUser);
        
        if(updatedUser == null) 
            return BadRequest($"Could not update user with Id : {request.Id}");
        return Ok(updatedUser);
    }
    
    #endregion
    //TODO: - create address for user
    //TODO: - update address
    //TODO: get /profile currently logged user 
    
    // private AppUser GetCurrentUser ()
    // {
    //     var identity = HttpContext.User.Identity as ClaimsIdentity;
    //
    //
    //     if (identity != null) {
    //
    //         var userClaims = identity.Claims;
    //
    //         return new AppUser {
    //             // UserName = userClaims.FirstOrDefault (x => x.Type == ClaimTypes.NameIdentifier)?.Value,
    //             // EmailAddress = userClaims.FirstOrDefault (x => x.Type == ClaimTypes.Email)?.Value,
    //             // Role = userClaims.FirstOrDefault (x => x.Type == ClaimTypes.Role)?.Value,
    //         };
    //
    //     }
    //     return null;
    
    // TODO: forgot psw
    [HttpPut("forgot-password")]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] ForgotPasswordBindingModel request)
    {
        if (request.Email.IsNullOrEmpty()) 
            return BadRequest($"Email cannot be empty");
        // send token to email to reset password link
        return Ok("for now !");
    }
    
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordBindingModel request)
    {
        AppUser user = await _userService.GetAsyncById(request.UserId);
        if(user == null)
            return BadRequest($"Could not find user with {request.UserId}");
        if (!_cryptoEngine.HashCheck(user.PasswordHash, request.OldPassword))
            return BadRequest($"Old password is incorrect !");
        
        bool result = await _userService.ChangePasswordAsync(user,request.NewPassword);
    
        if(!result) 
            return BadRequest($"Could not update password");
        return Ok($"Password has been changed");
    }
    
    #region DELETE

    [HttpDelete("{id}")]
    //[Authorize(Roles ="Admin")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        AppUser fetchedUser = await _userService.GetAsyncById(id);
        if(fetchedUser == null) BadRequest($"Could not find user with {id}");
        bool result = await _userService.DeleteAsync(fetchedUser.Id); 
        if(result == null) BadRequest($"Could not delete user with {id}");
        return Ok($"User with {id} has been deleted !");
    }
    #endregion
    
}