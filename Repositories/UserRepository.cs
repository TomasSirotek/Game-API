using System.Data;
using API.Identity.Entities;
using API.RepoInterface;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using NuGet.Protocol;

namespace API.Repositories; 

public class UserRepository : IUserRepository {

	private readonly List<AppUser> _appUsers;
	private readonly IConfiguration _config;
	
	public UserRepository (IConfiguration config)
	{
		_config = config;
	}
		public async Task<List<AppUser>> GetAllUsers()
		{
			
			// List<AppUser> userList = new();

			string sqlDataSource = _config.GetConnectionString("PostgresAppCon");
			using var con = new NpgsqlConnection(sqlDataSource);

			con.Open();

			string sql = "SELECT * FROM AspNetUsers";
			using var cmd = new NpgsqlCommand(sql, con);

			using NpgsqlDataReader rdr = cmd.ExecuteReader();

			while (rdr.Read())
			{
				Console.WriteLine("{0} {1} {2}", rdr.GetInt32(0), rdr.GetString(1),
					rdr.GetInt32(2));
			}
			
			return null;
		}

		public async Task<AppUser> GetUserById(string id)
		{
			var user = _appUsers.FirstOrDefault(x => x.Id == id);
			return user;
		}
		
		public  Task<bool> DeleteUser(string id)
		{
			string query = $"select * from AspNetUsers as u WHERE u.Id = @id ";
			// (delete from "AspNetUsers" AS t WHERE t."Id" = 'f64c7e2b-7d0e-4bd7-9f12-d9ffd5c727a6';)
			
			List<AppUser> userList = new();
			DataTable table = new DataTable();
			string sqlDataSource = _config.GetConnectionString("PostgresAppCon");
			NpgsqlDataReader appReader;
			using (NpgsqlConnection conn = new NpgsqlConnection(sqlDataSource))
			{
				conn.Open();
				using (NpgsqlCommand command = new NpgsqlCommand(query,conn))
				{
					appReader = command.ExecuteReader();
					table.Load(appReader);

					appReader.Close();
					conn.Close();
				
				}
			return null;
		}
		}
	
}
