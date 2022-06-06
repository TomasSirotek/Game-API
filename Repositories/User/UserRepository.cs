using System.Data;
using API.Dtos;
using API.Identity.Entities;
using API.Repositories;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using NuGet.Protocol;

namespace API.Repositories;

public class UserRepository : IUserRepository {
    private readonly IConfiguration _config;

    public UserRepository(IConfiguration config)
    {
        _config = config;
        // _userManager = userManager;
    }

    // Get all from db
    public async Task<List<AppUser>> GetAllUsers()
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            var sql = @"select * from app_user";

            IEnumerable<AppUser> newUser = await cnn.QueryAsync<AppUser>(sql);
            if (newUser != null)
            {
                return newUser.ToList();
            }
        }

        return null;
    }


    // get by id 
    public async Task<AppUser> GetUserById(string id)
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            var p = new DynamicParameters();
            p.Add("@id", id);

            var sql = @"select * from app_user as au where au.id = @id";

            AppUser user = await cnn.QueryFirstAsync<AppUser>(sql, p);
            if (user != null)
            {
                return user;
            }
        }

        return null;
    }
    
    // get by email
    public async Task<AppUser> GetAsyncByEmail(string email)
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            // write better params 
            var p = new DynamicParameters();
            p.Add("@email", email);

            var sql = @"select * from app_user as au where au.email = @email";

            AppUser user = await cnn.QueryFirstAsync<AppUser>(sql, p);
            if (user != null)
            {
                return user;
            }
        }

        return null;
    }


    // create user
    public async Task<AppUser> CreateUser(AppUser user, string passwordHash)
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            var sql = $@"INSERT INTO app_user (id,email,userName,firstName,lastName,passwordHash,isActivated,createdat,updatedat) 
                        values (@id,@email,@userName,@firstName,@lastName,@passwordHash,@isActivated,@createdat,@updatedat)";

            var newUser = await cnn.ExecuteAsync(sql, user);
            if (newUser > 0)
                return user;
        }


        return null;
    }

    public async Task<AppUser> AddToRoleAsync(AppUser user, AppRole role)
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            var sql = @"insert into user_role (userId,roleId) 
                        values (@id,@userId)";

            var newUser = await cnn.ExecuteAsync(sql, user);
            if (newUser > 1)
            {
                return user;
            }
        }

        return null;
    }


    // update user 


    // delete user
    public async Task<bool> DeleteUser(string id)
    {
        // AppUser user = await _userManager.FindByIdAsync(id);
        // if (user != null)
        // {
        //     IdentityResult result = await _userManager.DeleteAsync(user);
        //     if (result.Succeeded)
        //         return false;
        // }
        //
        return false;
    }
}