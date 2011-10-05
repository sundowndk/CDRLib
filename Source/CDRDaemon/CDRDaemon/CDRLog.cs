using System;
using System.Threading;
using System.IO;

namespace CDRDaemon
{
	public static class CDRLog
	{		
		private static readonly object _lock = new object();
		
		#region Private Static Methods
		public static void Write (string Record)
		{				
			lock (_lock)
			{
				StreamWriter writer = new StreamWriter (new FileStream (Runtime.Config.Get ("cdrlog", "path"), FileMode.Append, FileAccess.Write, FileShare.None));
				writer.WriteLine (Record);
				writer.Close ();
			}
		}
		#endregion

		#region Public Static Methods
		public static void Initialize ()
		{			
			StreamWriter writer;
			
			if (File.Exists (Runtime.Config.Get ("cdrlog", "path")))
			{
				writer = new StreamWriter (new FileStream (Runtime.Config.Get ("cdrlog", "path"), FileMode.Append, FileAccess.Write, FileShare.None));
				Logging.LogInfo ("Opening existing CDRLog : "+ Runtime.Config.Get ("cdrlog", "path"));			
			}
			else
			{
				writer = new StreamWriter (new FileStream (Runtime.Config.Get ("cdrlog", "path"), FileMode.Create, FileAccess.Write, FileShare.None));
				Logging.LogInfo ("Creating new CDRLog : "+ Runtime.Config.Get ("cdrlog", "path"));			
			}		
			
			writer.Close ();			
		}
		#endregion
	}
}