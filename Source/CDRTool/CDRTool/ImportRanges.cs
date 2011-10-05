using System;
using System.Text;
using System.Collections.Generic;

using CDRLib;
using Toolbox;

namespace CDRTool
{
	public class ImportRanges
	{
		public static void Test ()
		{
			Toolbox.CSVReader data = new Toolbox.CSVReader ("master.csv", Encoding.UTF8, ',', true);
			
			Console.WriteLine (data.Count);
			foreach (List<string> record in data)
			{
				string anumber = record[data.ColumnPos ("SOURCE")];
				string bnumber = record[data.ColumnPos ("DESTINATION")];
				int begintimestamp = Toolbox.Date.DateTimeToTimestamp (DateTime.Parse (record[data.ColumnPos ("STARTOFCALL")]));
				int duration = int.Parse (record[data.ColumnPos ("DURATION")]);

				
				switch (record[data.ColumnPos ("DESTINATIONDESCRIPTION")])
				{
				case "Incoming":
				{		
						
						SIPAccount sipaccount = SIPAccount.FindByNumber (bnumber);
						if (sipaccount != null)
						{
							Console.WriteLine ("Incomming call: ");
							Console.WriteLine ("\t From: "+ anumber +" to "+ bnumber);
							
							Usage usage = new Usage (Subscription.Load (sipaccount.SubscriptionId));
							usage.BeginTimestamp = begintimestamp;
							usage.Duration = duration;
							usage.ANumber = anumber;
							usage.BNumber = bnumber;
							usage.Direction = CDRLib.Enums.UsageDirection.Incomming;
							
							usage.Save ();
						}						
						
					break;
				}
						
				default:
				{
						SIPAccount sipaccount = SIPAccount.FindByNumber (anumber);
						if (sipaccount != null)
						{
							Console.WriteLine ("Outgoing call: ");
							Console.WriteLine ("\t From: "+ anumber +" to "+ bnumber);
							
							Usage usage = new Usage (Subscription.Load (sipaccount.SubscriptionId));
							usage.BeginTimestamp = begintimestamp;
							usage.Duration = duration;
							usage.ANumber = anumber;
							usage.BNumber = bnumber;
							usage.Direction = CDRLib.Enums.UsageDirection.Incomming;
							
							usage.Save ();
						}
					}
					break;
				}
				
				
					
				
				
				//				Console.WriteLine (data.ColumnPos ("SOURCE"));
			}
////				Console.WriteLine (ranges.ColumnPos ("name"));
////				ranges.ColumnPos ("name");
//				Console.WriteLine ("Name :"+ record[ranges.ColumnPos ("name")]);
//				Console.WriteLine ("\tDialcode: "+ record[ranges.ColumnPos ("dialcode")]);
//				Console.WriteLine ("\tCost: "+ record[ranges.ColumnPos ("cost")]);
//				
//				Range range = new Range ();
//				range.Name = record[ranges.ColumnPos ("name")];
//				range.DialCode = record[ranges.ColumnPos ("dialcode")];
//				
//				RangePrice cost = new RangePrice ();
//				cost.Price = decimal.Parse (record[ranges.ColumnPos ("cost")]);
//				
//				range.Cost.Add (cost);
//				range.Save ();
//			}
			
//			foreach (string data in Toolbox.IO.ReadTextFile ("ranges.csv", Encoding.UTF8))
//			{
//				Console.WriteLine (Toolbox.Tools.ParseCSVLine (data, ';')[1]);
//			}	
		}
		
		
		public ImportRanges ()
		{
			
		
			
		}
	}
}

