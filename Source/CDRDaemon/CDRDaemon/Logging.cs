using System;

namespace CDRDaemon
{
	public static class Logging
	{
		#region Public Static Fields
		public static Enums.LogLevel Level = Enums.LogLevel.Info | Enums.LogLevel.Warning | Enums.LogLevel.Error | Enums.LogLevel.FatalError;
		#endregion

		#region Private Static Methods
		private static void Write (Enums.LogLevel LogLevel, string Message)
		{
				Console.WriteLine ("["+ LogLevel.ToString ().ToUpper () +"]" + " "+ Message);
		}
		#endregion

		#region Public Static Methods
		public static void Initialize ()
		{
//			// Set loglevel
//			Level = (Enums.LogLevel)Enum.Parse (typeof (Enums.LogLevel), Services.Config.Get<string> ("core", "loglevel"), true);
			Level = Enums.LogLevel.FatalError | Enums.LogLevel.Error | Enums.LogLevel.Warning | Enums.LogLevel.Info | Enums.LogLevel.Debug;
		}

		public static void LogFatalError (string Message)
		{
			if ((Level & Enums.LogLevel.FatalError) == Enums.LogLevel.FatalError)
			{
				Write (Enums.LogLevel.FatalError, Message);
			}
		}

		public static void LogError (string Message)
		{
			if ((Level & Enums.LogLevel.Warning) == Enums.LogLevel.Warning)
			{
				Write (Enums.LogLevel.Warning, Message);
			}
		}

		public static void LogWarning (string Message)
		{
			if ((Level & Enums.LogLevel.Error) == Enums.LogLevel.Error)
			{
				Write (Enums.LogLevel.Error, Message);
			}
		}

		public static void LogInfo (string Message)
		{
			if ((Level & Enums.LogLevel.Info) == Enums.LogLevel.Info)
			{
				Write (Enums.LogLevel.Info, Message);
			}
		}

		public static void LogDebug (string Message)
		{
			if ((Level & Enums.LogLevel.Debug) == Enums.LogLevel.Debug)
			{
				Write (Enums.LogLevel.Debug, Message);
			}
		}
		#endregion
	}
}