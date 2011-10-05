using System;

namespace CDRDaemon.Enums
{
	[Flags]
	public enum LogLevel
	{
		None = 0,
		FatalError = 1,
		Error = 2,
		Warning = 4,
		Info = 8,
		Debug = 16
	}
}