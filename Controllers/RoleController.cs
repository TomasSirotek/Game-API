using System.Security.Claims;
using API.Data;
using API.Dtos;
using API.Identity.Entities;
using API.Identity.Services;
using API.Identity.Services.Role;
using API.Repositories;
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
    //[Authorize(Roles ="Admin")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllRoles ()
    {
         List<AppRole> userList =  await _roleService.GetAsync();
         if(userList.IsNullOrEmpty()) 
             return BadRequest($"Could not find any roles");
         return Ok(userList);
    }
    
    [HttpGet("{id}")]
    //[Authorize(Roles ="Admin")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRoleById (string id)
    {
        AppRole role =  await _roleService.GetAsyncById(id);
        if(role == null) 
            return BadRequest($"Could not role with Id: {id}");
        return Ok(role);
    }

    
    [HttpGet("name")]
    //[Authorize(Roles ="Admin")]
    public async Task<IActionResult> GetAsyncByName(string name)
    {
        AppRole role = await _roleService.GetAsyncByName(name);
        if (role == null) 
            return BadRequest($"Could not find role with name : {name}");
        return Ok (role);
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
        if(roleResult == null) 
            return BadRequest($"Could not create user with Email : {model.Name}");
        return Ok(roleResult);
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