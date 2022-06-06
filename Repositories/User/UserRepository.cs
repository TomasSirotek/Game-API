using System.Data;
using API.Identity.Entities;
using Dapper;
using Npgsql;

namespace API.Repositories;

public class UserRepository : IUserRepository {
    private readonly IConfiguration _config;

    public UserRepository(IConfiguration config)
    {
        _config = config;
    }

    // Get all from db
    public async Task<List<AppUser>> GetAllUsers()
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        { 
            var sql = @"SELECT *
                        FROM app_user u
                        inner JOIN user_role ur ON u.id = ur.userid 
                        inner JOIN role r ON ur.roleid = r.id";
            
            IEnumerable<AppUser> users = cnn.Query<AppUser, AppRole, AppUser>(sql, (u, r) =>
                { 
                    Dictionary<string, AppUser> userRoles = new Dictionary<string, AppUser>();
                    AppUser user;
                    if (!userRoles.TryGetValue(u.Id, out user))
                    {
                        userRoles.Add(u.Id, user = u);
                    }
            
                    if (user.Roles == null)
                        user.Roles = new List<AppRole>();
                    user.Roles.Add(r);
                    return user;
                },
                splitOn: "id"
            ) .GroupBy(u => u.Id)
                    .Select(group =>
                    {
                        AppUser user = group.First();
                        user.Roles = group.Select(u => u.Roles.Single()).ToList();
                        return user;
                    });
            return users.ToList();
        } 
    }


    // get by id 
    public async Task<AppUser> GetUserById(string id)
    {
    
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        { 
        var sql = @"SELECT *
                        FROM app_user u
                        inner JOIN user_role ur ON u.id = ur.userid 
                        inner JOIN role r ON ur.roleid = r.id
                        where u.id = @id";
        
        IEnumerable<AppUser> user = cnn.Query<AppUser, AppRole, AppUser>(sql, (u, r) =>
            {
                var userRoles = new Dictionary<string, AppUser>();
                AppUser user;
                if (!userRoles.TryGetValue(u.Id, out user))
                {
                    userRoles.Add(u.Id, user = u);
                }

                if (user.Roles == null)
                    user.Roles = new List<AppRole>();
                user.Roles.Add(r);
                return user;
            },
            new{Id = id}
        ).GroupBy(u => u.Id)
            .Select(group =>
            {
                AppUser user = group.First();
                user.Roles = group.Select(u => u.Roles.Single()).ToList();
                return user;
            });
            return user.First();
        }
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
    public async Task<AppUser> CreateUser(AppUser user)
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            var sql =
                $@"INSERT INTO app_user (id,email,userName,firstName,lastName,passwordHash,isActivated,createdat,updatedat) 
                        values (@id,@email,@userName,@firstName,@lastName,@passwordHash,@isActivated,@createdat,@updatedat)";

            var newUser = await cnn.ExecuteAsync(sql, user);
            if (newUser > 0)
            {
                return user;
            }
        }


        return null;
    }

    public async Task<AppUser> AddToRoleAsync(AppUser user, AppRole role)
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            var sql = @"insert into user_role (userId,roleId) 
                        values (@userId,@roleId)";

            var newUser = await cnn.ExecuteAsync(sql, new
            {
                UserId = user.Id,
                RoleId = role.Id
            });
            if (newUser > 0)
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
        return false;
    }
}