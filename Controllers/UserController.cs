using System.Data;
using System.Security.Claims;
using API.Data;
using API.Dtos;
using API.Identity.Entities;
using API.Identity.Services.User;
using API.Repositories;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using static API.Helpers.Tools;

namespace API.Controllers; 

public class UserController : DefaultController
{
    private readonly IAppUserService _userService;

    public UserController (IAppUserService userService)
    {
        _userService = userService;
    }
    
    #region GET
    [HttpGet("Admin")]
    //[Authorize(Roles ="Admin")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllUsers ()
    {
        List<AppUser> userList = await _userService.GetAllUsers();
        if (userList.IsNullOrEmpty())
            return BadRequest($"Could not find any users");
        return Ok(userList);
    }
    
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        AppUser user = await _userService.GetUserById(id);
        if (user != null) 
            return Ok (user);
        return BadRequest($"Could not find user with Id : {id}");
    }

    #endregion
    
    #region POST
    [HttpPost()]
    public async Task<IActionResult> CreateUser([FromBody]UserPostBindingModel model)
    {
        // move to services 
        AppUser user = new AppUser()
       {
           Id = Guid.NewGuid().ToString(),
           Email = model.Email,
           FirstName = model.FirstName,
           LastName = model.LastName,
           IsActivated = model.isActivated
           
       };
        AppUser resultUser = await _userService.CreateUser(user,model.Roles, model.Password);
        
        if(resultUser == null) 
            return BadRequest($"Could not create user with Email : {model.Email}");
        return Ok(resultUser);
    }
 
    
    #endregion
    //
    // #region PUT
    //
    // // create address from manager 
    //
    // // update address
    //
    // // get /profile currently logged user 
    //
    // // forgot psw
    //
    // // reset psw
    //
    //
    // // Update User 
    // [HttpPut()]
    // public async Task<IActionResult> UpdateUser(UserPutBindingModel model)
    // {
    //     // AppUser user = await _userManager.FindByEmailAsync(model.Email);
    //     // if (user != null)
    //     // {
    //     //     IdentityResult validPass = null;
    //     //     if (model.Password != null)
    //     //     {
    //     //         validPass = await _passwordValidator.ValidateAsync(_userManager, user, model.Password);
    //     //         if (validPass.Succeeded)
    //     //             user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
    //     //         else
    //     //             BadRequest($"Could not Update user psw: {validPass}");
    //     //     }
    //     //     user.UserName = model.UserName;
    //     //     
    //     //     IdentityResult result = await _userManager.UpdateAsync(user);
    //     //     if (result.Succeeded) return Ok(user);
    //    // }
    //    
    //  //  GetRolesAsync(AppUser user)
    //     return Ok ("role");
    //     
    //    // return BadRequest($"Could not find user with Id : {id}");
    // }
    //
    // #endregion
    //
    // [HttpDelete]
    // public async Task<IActionResult> DeleteUserById(string id)
    // {
    //     AppUser user = await _userManager.FindByIdAsync(id);
    //     if (user != null) return Ok($"User with id {id} was successfully deleted");
    //     
    //     return BadRequest($"Could not delete user with Id : {id}");
    // }
    //
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
    

}