using System.Data;
using API.Dtos;
using API.Identity.Entities;
using API.RepoInterface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using NuGet.Protocol;

namespace API.Repositories;

public class UserRepository : IUserRepository {
    private readonly List<AppUser> _appUsers;
    private readonly IConfiguration _config;
    private readonly NpgsqlConnection conn;
    private readonly UserManager<AppUser> _userManager;

    public UserRepository(IConfiguration config, UserManager<AppUser> userManager)
    {
        _config = config;
        _userManager = userManager;
    }

    public async Task<List<AppUser>> GetAllUsers()
    {
        return _userManager.Users.ToList();
    }

    public async Task<AppUser> GetUserById(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }


    public async Task<AppUser> CreateUser(AppUser user,string password)
    {
        if (user != null)
        {
             var result =  _userManager.CreateAsync(user,password);
             if (result.IsCompleted)
             {
                 AppUser newUser = await _userManager.FindByIdAsync(user.Id);
                 if (newUser != null) return newUser;
             }
             return null;
        }

        return null;
    }


    public async Task<bool> DeleteUser(string id)
    {
        AppUser user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            IdentityResult result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return false;
        }

        return false;
    }
}

