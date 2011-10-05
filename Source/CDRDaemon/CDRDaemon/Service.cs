using System;

using System.Threading;
using System.ServiceProcess;
using System.Collections.Generic;

using Toolbox;
using Toolbox.DBI;

namespace CDRDaemon
{
	class Service : ServiceBase
	{		
		#region Private Fields
		private bool _run = true;		
		#endregion
		
		#region Constructor
		public Service ()
		{
			this.ServiceName = "CDRDaemon";
			this.CanStop = true;
			this.CanPauseAndContinue = false;		
		}
		#endregion
			
		#region Public Static Methods
		public static void Start ()
		{
			Service service = new Service ();
			service.OnStart (new string[0]);
		}
		#endregion		
		
		#region Protected Methods
		protected override void OnStart (string [] args)
		{
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
			
			while (_run)
			{
//				Console.WriteLine ("Bla");
				Thread.Sleep (1000);
			}
		}

		protected override void OnStop ()
		{
			_run = false;
			Runtime.Shutdown ();
		}
		#endregion
	}
}

