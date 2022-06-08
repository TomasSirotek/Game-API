using System.Security.Claims;
using API.BindingModels.Role;
using API.Data;
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
    public async Task<IActionResult> GetAsync ()
    {
         List<AppRole> userList =  await _roleService.GetAsync();
         if(userList.IsNullOrEmpty()) 
             return BadRequest($"Could not find any roles");
         return Ok(userList);
    }
    
    [HttpGet("{id}")]
    //[Authorize(Roles ="Admin")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAsyncById (string id)
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
    public async Task<IActionResult> CreateAsync([FromBody]RolePostBindingModel model)
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
    
    [HttpPut()]
    public async Task<IActionResult> UpdateAsync([FromBody]RolePutBindingModel request)
    {
        if (string.IsNullOrWhiteSpace(request.Id) || string.IsNullOrWhiteSpace(request.Name))
            return BadRequest($"Could not create user with Id : {request.Id}");
        
        AppRole requestRole = new AppRole
        {
            Id = request.Id,
            Name = request.Name
        };
        AppRole updatedRole = await _roleService.UpdateAsync(requestRole);
        if (updatedRole == null) BadRequest($"Could not update role with {request.Id}");
        return Ok(updatedRole);
    }
    
    #endregion

    #region DELETE
    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        AppRole fetchedRole = await _roleService.GetAsyncById(id);
        if(fetchedRole == null) BadRequest($"Could not find role with {id}");
        bool result = await _roleService.DeleteAsync(fetchedRole.Id); 
        if(result == null) BadRequest($"Could not delete role with {id}");
         return Ok($"Role with {id} has been deleted !");
    }
    
    #endregion
   

}