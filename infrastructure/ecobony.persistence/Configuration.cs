using Microsoft.Extensions.Configuration;

namespace ecobony.persistence;

public static class Configuration
{
 public static string? ConnectionString
 {
  get
  {
   ConfigurationManager configurationManager = new ConfigurationManager();
   try
   {
    var extension = "../../presentation/ecobony.webapi";
    configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), extension));
    configurationManager.AddJsonFile("appsettings.json");
   }
   catch
   {
    configurationManager.AddJsonFile("appsettings.json");
   }

   return configurationManager.GetConnectionString("SQLServer");
  }
 }


 public static string ConnectionStringReddis
 {
  get
  {
   ConfigurationManager configurationManager = new();
   try
   {
    var extension = "../../presentation/ecobony.webapi";
    configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), extension));
    configurationManager.AddJsonFile("appsettings.json");
   }
   catch
   {
    configurationManager.AddJsonFile("appsettings.json");
   }

   return configurationManager.GetConnectionString("Reddis");
  }
 }
}