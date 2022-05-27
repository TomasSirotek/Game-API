using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.Identity.Entities;
using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers; 

public class UserController : DefaultController
{
    private readonly IUserInterface _user;
    private readonly UserManager<AppUser> _userManager;
    public UserController (IUserInterface user,UserManager<AppUser> userManager)
    {
        _user = user;
        _userManager = userManager;
    }

    [HttpGet("Admin")]
    [Authorize]
    //[Authorize(Roles ="Admin")]
    public async Task<IActionResult> GetAllUsers ()
    {
        var context = new DataContext(new DbContextOptions<DataContext>());
        var users = context.Users.ToList();
        return Ok(users);
    }
    
    [HttpGet("index")]
    public async Task<IActionResult> GetUserById(string id)
    {
        AppUser user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            return Ok (user);
        }

        return BadRequest($"Could not find user with Id : {id}");
    }
    
    public bool DeleteUser(string id)
    {
        // find by id if exist delete from repo 
        return false;
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