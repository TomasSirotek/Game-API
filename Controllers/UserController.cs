using System.Security.Claims;
using API.Data;
using API.Identity.Entities;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers; 

public class UserController : DefaultController
{
    private readonly IUserService _userService;
    private readonly UserManager<AppUser> _userManager;
    public UserController (IUserService userService,UserManager<AppUser> userManager)
    {
        _userService = userService;
        _userManager = userManager;
    }
    #region GET
    [HttpGet("Admin")]
    [Authorize]
    //[Authorize(Roles ="Admin")]
    public async Task<IActionResult> GetAllUsers ()
    {
        List<AppUser> userList = await _userService.GetAllUsers();
        if (userList != null) return Ok(userList);
        
        return BadRequest($"Could not find any users");
        // var context = new DataContext(new DbContextOptions<DataContext>());
        // var users = context.Users.ToList();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        AppUser user = await _userManager.FindByIdAsync(id);
        if (user != null) return Ok (user);
        
        return BadRequest($"Could not find user with Id : {id}");
    }
    #endregion
    
    [HttpDelete]
    public async Task<IActionResult> DeleteUserById(string id)
    {
        AppUser user = await _userManager.FindByIdAsync(id);
        if (user != null) return Ok($"User with id {id} was successfully deleted");
        
        return BadRequest($"Could not delete user with Id : {id}");
    }

    
    private AppUser GetCurrentUser ()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;


        if (identity != null) {

            var userClaims = identity.Claims;

            return new AppUser {
                // UserName = userClaims.FirstOrDefault (x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                // EmailAddress = userClaims.FirstOrDefault (x => x.Type == ClaimTypes.Email)?.Value,
                // Role = userClaims.FirstOrDefault (x => x.Type == ClaimTypes.Role)?.Value,
            };

        }
        return null;
    }

}