using System.Security.Claims;
using API.Data;
using API.Dtos;
using API.Identity.Entities;
using API.Identity.Managers;
using API.RepoInterface;
using API.Services.Interfaces;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers; 

public class RoleController : DefaultController
{
    private readonly IUserManager _userService;
    private readonly IUserRepository _userRepository;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IRoleManager _roleManager;
    private readonly UserManager<AppUser> _userManager;
    // private readonly UserValidator<AppUser> _userValidator;
    private readonly IPasswordHasher<AppUser> _passwordHasher;
    private readonly IPasswordValidator<AppUser> _passwordValidator;

    public RoleController (IUserManager userService,UserManager<AppUser> userManager,IUserRepository userRepository, SignInManager<AppUser> signInManager, IPasswordHasher<AppUser> passwordHasher,IPasswordValidator<AppUser> passwordValidator, IRoleManager roleManager)
    {
        _userService = userService;
        _userManager = userManager;
        _userRepository = userRepository;
        _signInManager = signInManager;
        _passwordHasher = passwordHasher;
        _passwordValidator = passwordValidator;
        _roleManager = roleManager;
    }
    #region GET
    [HttpGet()]
    // [Authorize]
    //[Authorize(Roles ="Admin")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllRoles ()
    {
         List<AppRole> userList =  await _roleManager.GetAllRoles();
         if(userList != null) return Ok(userList);
         return BadRequest($"Could not find any roles");
      
    }
    
    [HttpGet("id")]
    // [Authorize]
    //[Authorize(Roles ="Admin")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRoleById (string id)
    {
        AppRole role =  await _roleManager.GetRoleById(id);
        if(role != null) return Ok(role);
        return BadRequest($"Could not role with Id: {id}");
      
    }

    
    [HttpGet("name")]
    public async Task<IActionResult> GetRoleByName(string name)
    {
        AppUser user = await _userService.GetUserById(name);
        if (user != null) 
            return Ok (user);
        return BadRequest($"Could not find user with name : {name}");
    }
     
    #endregion

    #region POST

    // Create Role 
    
    [HttpPost()]
    public async Task<IActionResult> CreateRole([FromBody]RolePostBindingModel model)
    {
        
        var request = new AppRole()
        {
            Name = model.Name
        };
        AppRole roleResult = await _roleManager.CreateRole(request);
        
        return roleResult.Name != null ? Ok(request) : BadRequest($"Could not create role with Name : {model.Name}");
    }
    
    #endregion
    
    #region PUT
    
    // Update Role 
    
      
    [HttpPut()]
    public async Task<IActionResult> UpdateRole([FromBody]RolePutBindingModel model)
    {
        
        var request = new AppRole()
        {
            Id = model.Id,
            Name = model.Name
        };
        AppRole updatedRole = await _roleManager.UpdateRole(request);
        
        return updatedRole.Name != null ? Ok(request) : BadRequest($"Could not create role with Name : {model.Name}");
    }
    
    #endregion

    #region DELETE
    
    // Delete Role 
    
    #endregion
   

}