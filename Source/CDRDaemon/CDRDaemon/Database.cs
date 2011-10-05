using System;

using Toolbox.DBI;

namespace CDRDaemon
{
	public class Database
	{
		public static void Initialize ()
		{						
			CDRLib.Runtime.DBConnection = new Connection (Toolbox.Enums.DatabaseConnector.Mysql, Runtime.Config.Get ("database", "hostname"), Runtime.Config.Get ("database", "database"), Runtime.Config.Get ("database", "username"), Runtime.Config.Get ("database", "password"), true);
			CDRLib.Runtime.DBPrefix = Runtime.Config.Get ("database", "prefix");

			#region LOG:DEBUG
			Logging.LogDebug ("Connecting to mySQL server: HOSTNAME: "+ Runtime.Config.Get ("database", "hostname") +", DATABASE: "+ Runtime.Config.Get ("database", "database") +", USERNAME: "+ Runtime.Config.Get ("database", "username") +", PASSWORD: "+ Runtime.Config.Get ("database", "password") +", PREFIX: "+ Runtime.Config.Get ("database", "prefix"));			
			#endregion
		
			try
			{					
				if (CDRLib.Runtime.DBConnection.Connect ())
				{
					Logging.LogInfo ("Connected to database.");	
				}
				else
				{
					Logging.LogFatalError ("Could not establish connection to database server.");	
					Runtime.Shutdown ();
				}
			}
			catch (Exception exception)
			{
				Logging.LogFatalError ("An error occurred during database initialization.");					
				
				#region LOG:DEBUG
				Logging.LogDebug (exception.ToString ());
				#endregion
				
				Runtime.Shutdown ();
			}
		}
	}
}

