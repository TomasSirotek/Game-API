using System.Data;
using API.Identity.Entities;
using Dapper;
using Npgsql;

namespace API.Repositories;

public class RoleRepository : IRoleRepository {
    private readonly IConfiguration _config;

    public RoleRepository(IConfiguration config)
    {
        _config = config;
    }
    
    public async Task<List<AppRole>> GetAsync()
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            var sql = @"select * from role";

            IEnumerable<AppRole> newRole = await cnn.QueryAsync<AppRole>(sql);
            if (newRole != null)
            {
                return newRole.ToList();
            }
        }

        return null;
    }
    public async Task<AppRole> GetAsyncByName(string name)
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            var sql = @"select * from role as r where r.name = @name";

            AppRole role = await cnn.QueryFirstAsync<AppRole>(sql, new { Name = name});
            if (role != null)
            {
                return role;
            }
        }

        return null;
    }
    public async Task<AppRole> GetByIdAsync(string id)
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            var sql = @"select * from role as au where au.id = @id";

            AppRole user = await cnn.QueryFirstAsync<AppRole>(sql, new {Id = id});
            if (user != null)
            {
                return user;
            }
        }

        return null;
    }
    
    public async Task<bool> CreateAsync(AppRole role)
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            var sql = $@"insert into role (id,name) 
                        values (@id,@name)";

            var affectedRows = await cnn.ExecuteAsync(sql, role);
            if (affectedRows > 0)
            {
                return true;
            }

            return false;
        }
    }
    
    public async Task<AppRole> UpdateAsync(AppRole role)
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            var sql = $@"update role
                        SET name = @name 
                        where id = @id;";

            var affectedRows = await cnn.ExecuteAsync(sql, new
            {
                id = role.Id,
                name = role.Name
            });
            
            if (affectedRows > 0)
                return role;
            return null;
        }
    }

    public async Task<bool> DeleteAsync(string id)
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            
            var sql = $@"Delete 
                         from role 
                         where id = @id";
            
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