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
			string query = $"select * from AspNetUsers";

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

		public async Task<AppUser> GetUserById(string id)
		{
			var user = _appUsers.FirstOrDefault(x => x.Id == id);
			return user;
		}
		}
	
