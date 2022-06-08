using System.Data;
using API.Identity.Entities;
using Dapper;
using Npgsql;

namespace API.Repositories.Role;

public class RoleRepository : IRoleRepository {
    private readonly IConfiguration _config;
    private readonly IDbConnection _dbConnection;

    public RoleRepository(IConfiguration config,IDbConnection dbConnection)
    {
        _config = config;
        _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));

    }
    
    public async Task<List<AppRole>> GetAsync()
    {
        var sql = @"select * from role";

            IEnumerable<AppRole> newRole = await _dbConnection.QueryAsync<AppRole>(sql);
            if (newRole != null)
            {
                return newRole.ToList();
            }
  

        return null;
    }
    public async Task<AppRole> GetAsyncByName(string name)
    {
        var sql = @"select * from role as r where r.name = @name";

            AppRole role = await _dbConnection.QueryFirstAsync<AppRole>(sql, new { Name = name});
            if (role != null)
            {
                return role;
            }
        
        return null;
    }
    public async Task<AppRole> GetByIdAsync(string id)
    {
        var sql = @"select * from role as au where au.id = @id";

            AppRole user = await _dbConnection.QueryFirstAsync<AppRole>(sql, new {Id = id});
            if (user != null)
            {
                return user;
            }

            return null;
    }
    
    public async Task<bool> CreateAsync(AppRole role)
    {
        var sql = $@"insert into role (id,name) 
                        values (@id,@name)";

            var affectedRows = await _dbConnection.ExecuteAsync(sql, role);
            if (affectedRows > 0)
            {
                return true;
            }

            return false;
    }
    
    public async Task<AppRole> UpdateAsync(AppRole role)
    {
        var sql = $@"update role
                        SET name = @name 
                        where id = @id;";

            var affectedRows = await _dbConnection.ExecuteAsync(sql, new
            {
                id = role.Id,
                name = role.Name
            });
            
            if (affectedRows > 0)
                return role;
            return null;
      
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var sql = $@"Delete 
                         from role 
                         where id = @id";
            
            var newUser = await _dbConnection.ExecuteAsync(sql, new
            {
                Id = id
            });
            if (newUser > 0) 
                return true;
            return false;
    }
}