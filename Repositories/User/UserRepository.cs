using System.Data;
using API.Identity.Entities;
using Dapper;
using Npgsql;

namespace API.Repositories.User;

public class UserRepository : IUserRepository {
    
    private readonly IConfiguration _config;
    private readonly IDbConnection _dbConnection;

    public UserRepository(IConfiguration config,IDbConnection dbConnection)
    {
        _config = config;
        _dbConnection = dbConnection;
    }
    
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
                ).GroupBy(u => u.Id)
                .Select(group =>
                {
                    AppUser user = group.First();
                    user.Roles = group.Select(u => u.Roles.Single()).ToList();
                    return user;
                });
            return users.ToList();
        }
    }
    
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
                    new {Id = id}
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
    
    public async Task<AppUser> GetAsyncByEmail(string email)
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            var sql = @"SELECT *
                        FROM app_user u
                        inner JOIN user_role ur ON u.id = ur.userid 
                        inner JOIN role r ON ur.roleid = r.id
                        where u.email = @email";

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
                    new {Email = email}
                ).GroupBy(u => u.Id)
                .Select(group =>
                {
                    AppUser user = group.First();
                    user.Roles = group.Select(u => u.Roles.Single()).ToList();
                    return user;
                });
            AppUser[] appUsers = user as AppUser[] ?? user.ToArray();
            if (appUsers.Any())
                return appUsers.First();
            
        }
        return null;
    }
    
    public async Task<AppUser> CreateUser(AppUser user)
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            var sql =
                $@"INSERT INTO app_user (id,email,userName,firstName,lastName,passwordHash,isActivated,createdat,updatedat) 
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
                        values (@userId,@roleId)";

            var newUser = await cnn.ExecuteAsync(sql, new
            {
                UserId = user.Id,
                RoleId = role.Id
            });
            if (newUser > 0)
                return user;
            
        }
        return null;

    }

    public async Task<bool> SetActiveAsync(string id, bool result)
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            var sql = $@"update
                        app_user
                        set 
                        isActivated = @active
                        where id = @id;";
            
            var newUser = await cnn.ExecuteAsync(sql, new
            {
                Id = id,
                active = result
            });
            if (newUser > 0) 
                return true;

        }
        return false;
    }
    public async Task<bool> ChangePasswordAsync(AppUser user, string newPasswordHash)
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            var sql = $@"update
                        app_user
                        set 
                        passwordHash = @password
                        where id = @id;";
            
            var newUser = await cnn.ExecuteAsync(sql, new
            {
                id = user.Id,
                password = newPasswordHash
            });
            if (newUser > 0) 
                return true;

        }
        return false;
    }
    // update user 
    public async Task<AppUser> UpdateAsync(AppUser user)
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            
            var sql = $@"update
                        app_user
                        set 
                        email = @email,
                        username = @userName,
                        firstname = @firstName,
                        lastName = @lastName,
                        updatedat = current_timestamp
                        where id = @id;";
            
            var newUser = await cnn.ExecuteAsync(sql, new
            {
               id = user.Id,
               email = user.Email,
               username = user.UserName,
               firstName = user.FirstName,
               lastName = user.LastName

            });
            if (newUser > 0) 
                return user;

        }
        return null;
    }

    // delete user
    public async Task<bool> DeleteUser(string id)
    {
        
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            
            var sql = $@"Delete 
                         from user 
                         where id = @Id";
            
            var newUser = await cnn.ExecuteAsync(sql, new
            {
                Id = id
            });
            if (newUser > 0) 
                return true;
        }
        return false;
    }
}