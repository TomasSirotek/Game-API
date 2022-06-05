namespace API.Helpers;
using System.Configuration;


public class Tools {
    // look into this and fix it for more generic
    public static string GetConnectionString(string name = "PostgresAppCon")
    {
       // return ConfigurationManager.ConnectionStrings[name].ConnectionString;
      // return ConfigurationManager.ConnectionStrings[name].ConnectionString;
      return null;
    }
}