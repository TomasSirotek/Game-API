using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Identity.Entities;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers; 

public class UserController : DefaultController
{

    [HttpGet("Admin")]
    [Authorize(Roles ="Admin")]
    public IActionResult GetAllUsers ()
    {
       //  var currentUser = GetCurrentUser();
        // got service grab the function from repo 
       var test = "test";
       return Ok(test);
    }


    // GET: api/values
    [HttpGet("Public")]
    public IActionResult GetUserById()
    {
        return Ok ("Hi,public stuff");
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



    // GET api/values/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }



}