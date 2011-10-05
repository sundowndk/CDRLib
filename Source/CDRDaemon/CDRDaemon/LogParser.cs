using System;
using System.IO;
using System.Threading;

namespace CDRDaemon
{
	public class LogParser
	{
		
		public static void Monitor (object Path)
		{
			while (true)
			{
				
				Logging.LogInfo ("Started monitoring '"+ (string)Path +"'");
				StreamReader reader = new StreamReader (new FileStream ((string)Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
				
				
				Console.WriteLine ("test");
				
				string line = string.Empty;
				while ((line = reader.ReadLine()) != null)
				{
					Console.WriteLine (line);
				}
				
				
				
				reader.Close ();
				Logging.LogInfo ("Stopped monitoring '"+ (string)Path +"'");
				Thread.Sleep (1000);
			}
		}
	}
}

