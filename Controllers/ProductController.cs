using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using API.Dtos;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private static IConfiguration _config;
        
        public ProductController (IConfiguration config)
        {
            _config = config;
        }

        
        [HttpGet("TEST")]
        public JsonResult GetProducts()
        {
            string query =  $@"SELECT p.*,u.* 
            from customers p
            inner join users u on u.user_id = p.user_id
            where p.id = 1;
            ";
			     
            DataTable table = new DataTable();
            string sqlDataSource= _config.GetConnectionString("PostgresAppCon");
            NpgsqlDataReader appReader;
			     
            using (NpgsqlConnection appCon=new NpgsqlConnection(sqlDataSource))
            {
                appCon.Open();
                using (NpgsqlCommand appCommand=new NpgsqlCommand(query,appCon))
                {
                    appReader = appCommand.ExecuteReader();
                    table.Load(appReader);
                        
                    appReader.Close();
                    appCon.Close();
                }
                
            }
        
            return new JsonResult(table);
        }
        
        // [HttpGet("test")]
        // public ActionResult GetTest()
        // {
        //     string query =  @"select * from employee;";
			     //
        //     DataTable table = new DataTable();
        //     string sqlDataSource= _config.GetConnectionString("PostgresAppCon");
        //     NpgsqlDataReader appReader;
			     //
        //     using (NpgsqlConnection appCon=new NpgsqlConnection(sqlDataSource))
        //     {
        //         appCon.Open();
        //         using (NpgsqlCommand appCommand=new NpgsqlCommand(query,appCon))
        //         {
        //             appReader = appCommand.ExecuteReader();
        //             table.Load(appReader);
        //                 
        //             appReader.Close();
        //             appCon.Close();
        //         }
        //         
        //     }
        //
        //     return new JsonResult(table);
        // }
        //
        // public ActionResult<IEnumerable<EmployeeDto>> GetEmployees()
        // {
        //     string query =  @"select * from employee;";
			     //
        //     DataTable table = new DataTable();
        //     string sqlDataSource= _config.GetConnectionString("PostgresAppCon");
        //     NpgsqlDataReader appReader;
			     //
        //     using (NpgsqlConnection appCon=new NpgsqlConnection(sqlDataSource))
        //     {
        //         appCon.Open();
        //         using (NpgsqlCommand appCommand=new NpgsqlCommand(query,appCon))
        //         {
        //             appReader = appCommand.ExecuteReader();
        //             table.Load(appReader);
        //                 
        //             appReader.Close();
        //             appCon.Close();
        //         }
        //         
        //     }
        //
        //     return new JsonResult(table);
        // }
     }
 }

