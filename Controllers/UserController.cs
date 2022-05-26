using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Identity.Entities;
using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers; 

public class UserController : DefaultController
{
    private readonly IUserInterface _user;

    public UserController (IUserInterface user)
    {
        _user = user;
    }

    [HttpGet("Admin")]
    //[Authorize(Roles ="Admin")]
    public async Task<IActionResult> GetAllUsers ()
    {
        List<AppUser> users = await _user.GetAllUsers();
        return Ok(users);
    }
    
    [HttpGet("index")]
    public async Task<IActionResult> GetUserById(string id)
    {
        AppUser user = await _user.GetUserById(id);
        return Ok (user);
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