using System;
using System.ServiceProcess;
using System.Threading;

using Toolbox;

namespace CDRDaemon
{
	class MainClass
	{		
		public static void Main (string[] args)
		{							
			Toolbox.Arg.Add ("conf", "c", "Look for cdrdaemon.conf file in this {0}", true, "PATH", "conf");
//			Toolbox.Arg.Add ("debug", string.Empty, "Do not send to background and redirect log to stdout. LogLevel will be set to DEBUG", "debug");			
			Toolbox.Arg.Add ("help", "h", "Show this help", "help");
					
			Arg.Parse (args);
			
			System.ServiceProcess.ServiceBase.Run (new Service ());
			
//			Console.WriteLine ("CDRDaemon v"+ Runtime.VersionString +" ["+ Runtime.CompileDate +"]\n");
//			Console.WriteLine ("This is a mono service, please use mono-service2 to manage it.\n");
											
			if (Toolbox.Arg.Found ("help"))
			{
				Console.WriteLine("Usage: cdrdaemon [OPTIONS]");
				Console.WriteLine (Toolbox.Arg.Description);
				Environment.Exit (0);
			} 
			
// Set processname, makes it easier to find in the process list.
			Runtime.SetProcessName ("CDRDaemon");
			
			// Initialize configuration.
			Configuration.Initialize ();
			
			// Initialize logging.
			Logging.Initialize ();					
			Logging.LogInfo ("CDRDeamon v"+ Runtime.VersionString + " ["+ Runtime.CompileDate +"] started.");			
			
			// Initialize cdrlog
			CDRLog.Initialize ();			
			
			// Initialize database.
			Database.Initialize ();								
								
			// Initialize monitor threads.
			foreach (Config.Section section in Runtime.Config.GetSections ("monitor"))
			{
				Thread thread = new Thread (new ParameterizedThreadStart(Worker.MonitorFolder));
				thread.Start (section.Get ("path"));
			}
			
			while (true)
			{
//				Console.WriteLine ("Bla");
				Thread.Sleep (1000);
			}			
			
			
//			if (Toolbox.Arg.Found ("debug"))
//			{
//				Service.Start ();				
//			}						
		}
	}
}

