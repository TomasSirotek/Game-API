using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Identity.Entities;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        [HttpGet("Admin")]
        [Authorize]
        public IActionResult AdminEndpoint ()
		{
            var currentUser = GetCurrentUser();

           // return Ok ($"Auth ok your are logged as {currentUser.UserName}, Role : {currentUser.Role}");
           return Ok();
        }


        // GET: api/values
        [HttpGet("Public")]
        public IActionResult Public()
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
}

