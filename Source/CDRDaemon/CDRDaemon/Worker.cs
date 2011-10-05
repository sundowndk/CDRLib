using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

using CDRLib;

namespace CDRDaemon
{
	public class Worker
	{		
		
		
		public static void MonitorFolder (object Path)
		{
			FileSystemWatcher watcher = new FileSystemWatcher ();
			watcher.Path = (string)Path;
			watcher.Created += HandleWatcherChanged;
			watcher.NotifyFilter = NotifyFilters.LastWrite;
			watcher.EnableRaisingEvents = true;		
			
			Logging.LogInfo ("Started monitoring 'folder' '"+ (string)Path +"'");
			
//			while (true)
//			{			
//				Logging.LogInfo ("Started monitoring '"+ (string)Path +"'");
//				StreamReader reader = new StreamReader (new FileStream ((string)Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
//				
//				
//				Console.WriteLine ("test");
//				
//				string line = string.Empty;
//				while ((line = reader.ReadLine()) != null)
//				{
//					Console.WriteLine (line);
//				}
//				
//				
//				
//				reader.Close ();
//				Logging.LogInfo ("Stopped monitoring '"+ (string)Path +"'");
//				Thread.Sleep (1000);
//			}
		}
		
		static private void ParseFile (string Path)
		{
			Logging.LogDebug ("Parsing file: '"+ Path +"'");
						
			StreamReader reader = new StreamReader (new FileStream (Path, FileMode.Open, FileAccess.Read, FileShare.None));
			string line = string.Empty;
			while ((line = reader.ReadLine()) != null)
			{
				ParseLine (line);
//				Console.WriteLine (line);
			}				
			reader.Close ();		
							
			File.Delete (Path);
		}
		
		static private void ParseLine (string Line)
		{
			List<string> record = CSVReader.Parse (Line, ',');
			
			string anumber = record[1];
			string bnumber = record[2];
			int begintimestamp = Toolbox.Date.DateTimeToTimestamp (DateTime.Parse (record[9]));
			int duration = int.Parse (record[12]);
			
			switch (record[3])
			{
				case "Incoming":
				{							
					SIPAccount sipaccount = SIPAccount.FindByNumber (bnumber);
//					if (sipaccount != null)
//					{						
//						Usage usage = new Usage (Subscription.Load (sipaccount.SubscriptionId));
//						usage.BeginTimestamp = begintimestamp;
//						usage.Duration = duration;
//						usage.ANumber = anumber;
//						usage.BNumber = bnumber;
//						usage.Direction = CDRLib.Enums.UsageDirection.Incomming;
//							
//						usage.Save ();
//						
//						#region LOG:DEBUG
//						Logging.LogDebug ("Incomming call from "+ anumber +" to "+ bnumber +", Duration: "+ duration);
//						#endregion						
//					}											
					break;
				}
					
				default:
				{
//					Console.WriteLine (anumber);
					SIPAccount sipaccount = SIPAccount.FindByNumber (anumber);
//					if (sipaccount != null)
//					{
//						Usage usage = new Usage (Subscription.Load (sipaccount.SubscriptionId));
//						usage.BeginTimestamp = begintimestamp;
//						usage.Duration = duration;
//						usage.ANumber = anumber;
//						usage.BNumber = bnumber;
//						usage.Direction = CDRLib.Enums.UsageDirection.Incomming;
//							
//						usage.Save ();
//						
//						#region LOG:DEBUG
//						Logging.LogDebug ("Outgoing call from "+ anumber +" to "+ bnumber +", Duration: "+ duration);
//						#endregion
//					}
					break;
				}
			}			
			
			CDRLog.Write (Line);
		}

		static void HandleWatcherChanged (object Sender, FileSystemEventArgs EventArgs)
		{			
			#region LOG:DEBUG
			Logging.LogDebug ("New file detected: '"+ EventArgs.FullPath +"'");
			#endregion
						
			ParseFile (EventArgs.FullPath);
		}
	}
}

