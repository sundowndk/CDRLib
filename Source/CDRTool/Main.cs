using System;
using System.IO;

using Toolbox.DBI;

namespace CDRTool
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			CDRLib.Runtime.DBConnection = new Connection (	Toolbox.Enums.DatabaseConnector.Mysql,
															"172.20.0.2",
															"cdrtool",
															"cdrtool",
															"PAss1234",
															true);
			
			if (CDRLib.Runtime.DBConnection.Connect ())
			{							
//				CDRTool.ImportRanges.Test ();
				
		
				StreamReader reader = new StreamReader (new FileStream ("/home/sundown/test.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
				string line = string.Empty;
				while ( (line = reader.ReadLine()) != null )
				{
					Console.WriteLine(line);					
				}
				reader.Close ();

					
//				using ( StreamReader reader = new StreamReader(new FileStream(fileName, 
//                     FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) )
//{
//    //start at the end of the file
//
//    long lastMaxOffset = reader.BaseStream.Length;
//
//    while ( true )
//    {
//        System.Threading.Thread.Sleep(100);
//
//        //if the file size has not changed, idle
//
//        if ( reader.BaseStream.Length == lastMaxOffset )
//            continue;
//
//        //seek to the last max offset
//
//        reader.BaseStream.Seek(lastMaxOffset, SeekOrigin.Begin);
//
//        //read out of the file until the EOF
//
//        string line = "";
//        while ( (line = reader.ReadLine()) != null )
//            Console.WriteLine(line);
//
//        //update the last max offset
//
//        lastMaxOffset = reader.BaseStream.Position;
//    }
//}				
				
			}
			else
			{				
				Console.WriteLine ("Could not establish database connection.");
			}	
		}
	}
}
