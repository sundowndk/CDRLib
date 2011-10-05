using System;

using Toolbox;

namespace CDRDaemon
{
	public class Configuration
	{
		public static void Initialize ()
		{
			string confpath = "/home/sundown/cdrdaemon.conf";
			
			if (Toolbox.Arg.Found ("conf"))
			{				
			}
									
			Runtime.Config = new Config (confpath);
		}
	}
}

