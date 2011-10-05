using System;
using System.Threading;

namespace CDRDaemon
{
	public class WorkerLogParser
	{		
		public static void Start ()
		{
			while (true)
			{
				Console.WriteLine ("Still here...");
				Thread.Sleep (1000);
			}
		}
	}
}

