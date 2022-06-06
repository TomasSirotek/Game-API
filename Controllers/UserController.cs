using System.Data;
using System.Security.Claims;
using API.Data;
using API.Dtos;
using API.Identity.Entities;
using API.Repositories;
using API.Services.Interfaces;
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
    private readonly IConfiguration _config;
  //  private readonly IDefaultUserManager _userService;
    // private readonly IUserRepository _userRepository;
    // private readonly SignInManager<AppUser> _signInManager;
    // private readonly UserManager<AppUser> _userManager;
    // // private readonly UserValidator<AppUser> _userValidator;
    // private readonly IPasswordHasher<AppUser> _passwordHasher;
    // private readonly IPasswordValidator<AppUser> _passwordValidator;
    // private readonly RoleManager<AppRole> _roleManager;
    private readonly IAppUserService _userService;

    public UserController ( IConfiguration config,IAppUserService userService)
    {
        //_userService = userService;
        // _userManager = userManager;
        // _userRepository = userRepository;
        // _signInManager = signInManager;
        // _passwordHasher = passwordHasher;
        // _passwordValidator = passwordValidator;
        // _roleManager = roleManager;
        _config = config;
        _userService = userService;
    }
    #region GET
    [HttpGet("Admin")]
    // [Authorize]
    //[Authorize(Roles ="Admin")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllUsers ()
    {
        List<AppUser> userList = await _userService.GetAllUsers();
        if (userList !=  null) return Ok(userList);
        
        return BadRequest($"Could not find any users");

    }
    
    #endregion
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        AppUser user = await _userService.GetUserById(id);
        if (user != null) 
            return Ok (user);
        return BadRequest($"Could not find user with Id : {id}");
    }

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
        
       return resultUser.Email != null ? Ok(user) : BadRequest($"Could not create user with Email : {model.Email}");
 
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