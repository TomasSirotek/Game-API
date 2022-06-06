using System.Security.Claims;
using API.Data;
using API.Dtos;
using API.Identity.Entities;
using API.Identity.Services;
using API.Repositories;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers; 

public class RoleController : DefaultController
{

    private readonly IAppRoleService _roleService;
  

    public RoleController (IAppRoleService roleService)
    {
        _roleService = roleService;
    }
    #region GET
    [HttpGet()]
    // [Authorize]
    //[Authorize(Roles ="Admin")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllRoles ()
    {
         List<AppRole> userList =  await _roleService.GetAsync();
         if(userList != null) return Ok(userList);
         return BadRequest($"Could not find any roles");
      
    }
    
    [HttpGet("{id}")]
    // [Authorize]
    //[Authorize(Roles ="Admin")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRoleById (string id)
    {
        AppRole role =  await _roleService.GetAsyncById(id);
        if(role != null) return Ok(role);
        return BadRequest($"Could not role with Id: {id}");
      
    }

    
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetAsyncByName(string name)
    {
        AppRole role = await _roleService.GetAsyncByName(name);
        if (role != null) 
            return Ok (role);
        return BadRequest($"Could not find role with name : {name}");
    }
     
    #endregion
    
    // Create Role 
    #region POST
    [HttpPost()]
    public async Task<IActionResult> CreateRole([FromBody]RolePostBindingModel model)
    {
        // move to services 
        AppRole modelRole = new AppRole()
        {
            Id = Guid.NewGuid().ToString(),
            Name = model.Name
        };
        AppRole roleResult = await _roleService.CreateAsync(modelRole);
        
        return roleResult != null ? Ok(roleResult) : BadRequest($"Could not create user with Email : {model.Name}");
 
    }
  
    
    #endregion
    
    #region PUT
    
    // Update Role 
    [HttpPut()]
    public async Task<IActionResult> UpdateRole([FromBody]RolePutBindingModel model)
    {
        
        // var request = new AppRole()
        // {
        //     Id = model.Id,
        //     Name = model.Name
        // };
        // AppRole updatedRole = await _roleManager.UpdateRole(request);
        //
        // return updatedRole.Name != null ? Ok(request) : BadRequest($"Could not create role with Name : {model.Name}");
        return null;
    }
    
    #endregion

    #region DELETE
    
    // Delete Role 
    
    #endregion
   

}