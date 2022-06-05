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
    public async Task<AppRole> GetByIdAsync(string id)
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            var p = new DynamicParameters();
            p.Add("@id", id);

            var sql = @"select * from role as au where au.id = @id";

            AppRole user = await cnn.QueryFirstAsync<AppRole>(sql, p);
            if (user != null)
            {
                return user;
            }
        }

        return null;
    }
    
    public async Task<AppRole> CreateAsync(AppRole role)
    {
        using (IDbConnection cnn = new NpgsqlConnection(_config.GetConnectionString("PostgresAppCon")))
        {
            var sql = @"insert into role (id,name) 
                        values (@id,@name)";
            
            var result = await cnn.ExecuteAsync(sql, role);
            if (result > 1)
            {
                return role;
            }

            return null;
        }
    }
}