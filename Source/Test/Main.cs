using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using CDRLib;
using Toolbox.DBI;

namespace Test
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
				
				foreach (Range range in Range.List ())
				{
					Console.WriteLine (range.Name);

					RangePrice p1 = new RangePrice ();
					p1.Price = 1.5m;
					p1.HourSpanBegin = "08:00";
					p1.HourSpanEnd = "19:59";
					p1.Weekdays = CDRLib.Enums.Weekday.All;
					p1.Save ();
					
					RangePrice p2 = new RangePrice ();
					p2.Price = 1.1m;
					p2.HourSpanBegin = "20:00";
					p2.HourSpanEnd = "07:59";
					p2.Weekdays = CDRLib.Enums.Weekday.All;
					p2.Save ();

					RangePrice p3 = new RangePrice ();
					p3.Price = 1.1m;
					p3.HourSpanBegin = "00:00";
					p3.HourSpanEnd = "00:00";
					p3.Weekdays = CDRLib.Enums.Weekday.Sunday;
					p3.Save ();						
					
					RangePriceGroup g1 = new RangePriceGroup ();
					g1.RangePrices.Add (p1);
					g1.RangePrices.Add (p2);
					g1.RangePrices.Add (p3);
					g1.Name = "Priser 2011";
					g1.ValidFromTimestamp = Toolbox.Date.DateTimeToTimestamp (DateTime.Parse ("2011-01-01 00:00:00"));
					g1.ValidToTimestamp = Toolbox.Date.DateTimeToTimestamp (DateTime.Parse ("2011-12-13 23:59:00"));
					g1.Save ();
					
					RangePrice p4 = new RangePrice ();
					p4.Price = 2.5m;
					p4.HourSpanBegin = "08:00";
					p4.HourSpanEnd = "19:59";
					p4.Weekdays = CDRLib.Enums.Weekday.All;
					p4.Save ();
					
					RangePrice p5 = new RangePrice ();
					p5.Price = 2.1m;
					p5.HourSpanBegin = "20:00";
					p5.HourSpanEnd = "07:59";
					p5.Weekdays = CDRLib.Enums.Weekday.All;
					p5.Save ();

					RangePrice p6 = new RangePrice ();
					p6.Price = 2.1m;
					p6.HourSpanBegin = "00:00";
					p6.HourSpanEnd = "00:00";
					p6.Weekdays = CDRLib.Enums.Weekday.Sunday;
					p6.Save ();						
					
					RangePriceGroup g2 = new RangePriceGroup ();
					g2.RangePrices.Add (p4);
					g2.RangePrices.Add (p5);
					g2.RangePrices.Add (p6);
					g2.Name = "Priser 2011";
					g2.ValidFromTimestamp = Toolbox.Date.DateTimeToTimestamp (DateTime.Parse ("2011-01-01 00:00:00"));
					g2.ValidToTimestamp = Toolbox.Date.DateTimeToTimestamp (DateTime.Parse ("2011-12-13 23:59:00"));
					g2.Save ();
					
					
					range.CostRangePriceGroups.Add (g1);
					range.RetailRangePriceGroups.Add (g2);
					
					range.Save ();
					
				}
				
//				RangePrice p1 = new RangePrice ();
//				p1.Price = 1.5m;
//				p1.HourSpanBegin = "08:00";
//				p1.HourSpanEnd = "19:59";
//				p1.Weekdays = CDRLib.Enums.Weekday.All;
//				p1.Save ();
//				
//				RangePrice p2 = new RangePrice ();
//				p2.Price = 1.1m;
//				p2.HourSpanBegin = "20:00";
//				p2.HourSpanEnd = "07:59";
//				p2.Weekdays = CDRLib.Enums.Weekday.All;
//				p2.Save ();
//
//				RangePrice p3 = new RangePrice ();
//				p3.Price = 1.1m;
//				p3.HourSpanBegin = "00:00";
//				p3.HourSpanEnd = "00:00";
//				p3.Weekdays = CDRLib.Enums.Weekday.Sunday;
//				p3.Save ();								
//				
//				RangePriceGroup g1 = new RangePriceGroup ();
//				g1.RangePrices.Add (p1);
//				g1.RangePrices.Add (p2);
//				g1.RangePrices.Add (p3);
//				g1.Name = "Priser 2011";
//				g1.ValidFromTimestamp = Toolbox.Date.DateTimeToTimestamp (DateTime.Parse ("2011-01-01 00:00:00"));
//				g1.ValidToTimestamp = Toolbox.Date.DateTimeToTimestamp (DateTime.Parse ("2011-12-13 23:59:00"));
//				g1.Save ();
				
				
				
				
				
				
//				Subscription subscription1 = Subscription.Load (new Guid ("8ff83b88-77f8-4c29-94fe-30382518c967"));
				
//				Customer customer1 = Customer.Load (new Guid ("39b117af-26d6-4238-b6ef-76b9032e745f"));
//				customer1.Subscriptions.Add (subscription1);
//				customer1.Save ();
				
//				Console.WriteLine (customer1.Name);
//				foreach (Subscription subscription in customer1.Subscriptions)
//				{
//					foreach (SIPAccount sipaccount in subscription.SIPAccounts)
//					{
//						foreach (string number in sipaccount.Numbers)
//						{
//							Console.WriteLine (number);		
//						}
//					}
//				}
				
//				Customer customer1 = new Customer ();
//				customer1.Name = "VIP GROUP";
//				customer1.Save ();
				
				
//				Subscription subscription1 = new Subscription (customer1);
//				subscription1.Save ();
//				
//				SIPAccount sipaccount1 = new SIPAccount (subscription1);
//				sipaccount1.Numbers.Add ("88334660");
//				sipaccount1.Numbers.Add ("20210844");
//				sipaccount1.Numbers.Add ("50460609");
//				sipaccount1.Numbers.Add ("30336439");				
//				
//				sipaccount1.Save ();
//				
//				RangeGroup r1 = RangeGroup.Load (new Guid ("2a17c788-a00a-48c2-980d-ef06569d6688"));
//		
//				Console.WriteLine ("RangeGroup: "+ r1.Name);
//				Console.WriteLine (r1.CountryCodes.Count);
//				
//				foreach (CountryCode cc in r1.CountryCodes)
//				{					
//					foreach (string dc in cc.DialCodes)
//					{
//						Console.WriteLine (dc);
//					}
//				}
				
//				RangeGroup r1 = new RangeGroup ();
//				r1.Name = "TerraSip";
//				
//				List<Guid> ids = new List<Guid> ();
//				ids.Add (new Guid ("cbf25949-3daa-4a5b-a1e3-bc9f67157d75"));
//				ids.Add (new Guid ("266bbaec-19a1-41eb-a900-f6a4de7f6b1d"));
//				ids.Add (new Guid ("66153278-43f4-43f7-b223-09a3cbdad0ce"));
//				ids.Add (new Guid ("4476b7de-14c6-4f77-8000-aebc709c6ef3"));
//				ids.Add (new Guid ("1d1e9f7d-9738-46e5-8b97-f6ae2b3835db"));
//				ids.Add (new Guid ("16164d30-e8ee-4d19-ae28-880290f03282"));
//				ids.Add (new Guid ("0186ead3-1a3b-46d1-b966-25dcdf792679"));
//				ids.Add (new Guid ("3fea0771-e9ea-466f-aa33-930000a598c3"));
//				ids.Add (new Guid ("8133317f-31f6-4092-8c94-747aef38e69b"));
//				ids.Add (new Guid ("b6032176-c850-43e7-ae24-d2bfcb868aba"));
//				
//				foreach (RangeGroup r2 in RangeGroup.List ())
//				{
//					if (!ids.Contains (r2.Id))
//					{
//						Console.WriteLine (r2.Name);
//						r1.Ranges.Add (r2);						
//					}
//				}
//				r1.Save ();
						
//cbf25949-3daa-4a5b-a1e3-bc9f67157d75 Norden
//266bbaec-19a1-41eb-a900-f6a4de7f6b1d Europa 1
//66153278-43f4-43f7-b223-09a3cbdad0ce Europa 2
//4476b7de-14c6-4f77-8000-aebc709c6ef3 Europa 3
//1d1e9f7d-9738-46e5-8b97-f6ae2b3835db Asien
//16164d30-e8ee-4d19-ae28-880290f03282 Øvrige Verden 1
//0186ead3-1a3b-46d1-b966-25dcdf792679 Øvrige Verden 2
//3fea0771-e9ea-466f-aa33-930000a598c3 Øvrige Verden 3
//8133317f-31f6-4092-8c94-747aef38e69b Nordamerika				
				
				
//				RangeGroup r1 = new RangeGroup ();
//				r1.Name = "Nordamerika";
//				
//				foreach (string line in Toolbox.IO.ReadTextFile ("ipvisiongrupper", Encoding.UTF8))
//				{					
//					foreach (RangeGroup rangegroup in RangeGroup.List ())
//					{
//						if (rangegroup.Name == line.Trim ())
//						{							
//							Console.WriteLine (rangegroup.Name);
//							r1.Ranges.Add (rangegroup);
//						}
//					}
//				}
//				
//				Console.WriteLine (r1.Ranges.Count);
////				r1.Save ();

				Environment.Exit (0);
				
//				List<string> test = new List<string> ();
//				foreach (string line in Toolbox.IO.ReadTextFile ("countrycodes.csv", Encoding.UTF8))
//				{									
//					string[] split = line.Split (";".ToCharArray ());
//					
//					CountryCode countrycode = new CountryCode ();
//					countrycode.Name = split[0];
//					
//					Console.WriteLine ("Country: "+ split[0]);
//					foreach (string code in split[1].Split (",".ToCharArray (), StringSplitOptions.RemoveEmptyEntries))
//					{
//						foreach (string name in split[2].Split (",".ToCharArray (), StringSplitOptions.RemoveEmptyEntries))
//						{
//							
//							countrycode.AlternativNames.Add (name);
//							Console.WriteLine ("\tAlternativName: "+ name);
//						}
//						
//						Console.WriteLine ("\tCode: "+ code);
//						countrycode.Codes.Add (code);
//						test.Add (code);
//					}
//					countrycode.Save ();
//					
//				}
				
				
//				Environment.Exit (0);
//				Range range1 = new Range ();
//				range1.Name = "Danmark";
//				
//				RangePrice rangeprice1 = new RangePrice ();
//				rangeprice1.Price = 2.5m;
//				rangeprice1.HourSpanBegin = "08:00";
//				rangeprice1.HourSpanEnd = "20:00";
//				rangeprice1.Weekdays = CDRLib.Enums.Weekday.All;
//				
//				RangePrice rangeprice2 = new RangePrice ();
//				rangeprice2.Price = 1.2m;
//				rangeprice2.HourSpanBegin = "20:00";
//				rangeprice2.HourSpanEnd = "08:00";
//				rangeprice2.Weekdays = CDRLib.Enums.Weekday.All;
//				
//				RangePrice rangeprice3 = new RangePrice ();
//				rangeprice3.Price = 1.2m;
//				rangeprice3.HourSpanBegin = "00:00";
//				rangeprice3.HourSpanEnd = "00:00";
//				rangeprice3.Weekdays = CDRLib.Enums.Weekday.Sunday;
//				
//				RangePrice rangeprice4 = new RangePrice ();
//				rangeprice4.Price = 2.8m;
//				rangeprice4.HourSpanBegin = "08:00";
//				rangeprice4.HourSpanEnd = "20:00";
//				rangeprice4.Weekdays = CDRLib.Enums.Weekday.All;
//				
//				RangePrice rangeprice5 = new RangePrice ();
//				rangeprice5.Price = 1.5m;
//				rangeprice5.HourSpanBegin = "20:00";
//				rangeprice5.HourSpanEnd = "08:00";
//				rangeprice5.Weekdays = CDRLib.Enums.Weekday.All;
//				
//				RangePrice rangeprice6 = new RangePrice ();
//				rangeprice6.Price = 1.5m;
//				rangeprice6.HourSpanBegin = "00:00";
//				rangeprice6.HourSpanEnd = "00:00";
//				rangeprice6.Weekdays = CDRLib.Enums.Weekday.Sunday;				
//
//				range1.Cost.Add (rangeprice1);
//				range1.Cost.Add (rangeprice2);
//				range1.Cost.Add (rangeprice3);
//				
//				range1.Retail.Add (rangeprice4);
//				range1.Retail.Add (rangeprice5);
//				range1.Retail.Add (rangeprice6);
//				
//				range1.DialCode = "4535";
//				range1.Save ();
								
//				foreach (Range range in Range.List ())
//				{
//					Console.WriteLine ("Range: "+ range.Name);
//					Console.WriteLine ("\t Cost: ");
//					foreach (RangePrice rangeprice in range.Cost)
//					{
//						Console.WriteLine ("\t\t Price: "+ rangeprice.Price);
//						Console.WriteLine ("\t\t HourSpanBegin: "+ rangeprice.HourSpanBegin);
//						Console.WriteLine ("\t\t HourSpanEnd: "+ rangeprice.HourSpanEnd);
//						Console.WriteLine ("\t\t Weekdays: "+ rangeprice.Weekdays);
//						Console.WriteLine ("");
//					}
//					
//					Console.WriteLine ("\t Retail: ");
//					foreach (RangePrice rangeprice in range.Retail)
//					{
//						Console.WriteLine ("\t\t Price: "+ rangeprice.Price);
//						Console.WriteLine ("\t\t HourSpanBegin: "+ rangeprice.HourSpanBegin);
//						Console.WriteLine ("\t\t HourSpanEnd: "+ rangeprice.HourSpanEnd);
//						Console.WriteLine ("\t\t Weekdays: "+ rangeprice.Weekdays);
//						Console.WriteLine ("");
//					}					
//					
//					Console.WriteLine ("\t Dialcode: "+ range.DialCode);
//				}				
				
//				foreach (RangeGroup rangegroup in RangeGroup.List ())
//				{
//					Console.WriteLine (rangegroup.Id);
//					RangeGroup.Delete (rangegroup.Id);
//				}
				
//				RangeGroup rangegroup1 = new RangeGroup ();
//				rangegroup1.Name = "All";
//				
//				
			
//			foreach (CountryCode countrycode in CountryCode.List ())
//			{
//				Console.WriteLine (countrycode.Name);
//				
//			}
		
				
				
				
//				foreach (RangeGroup rangegroup in RangeGroup.List ())
//				{
//					Console.WriteLine ("RangeGroup: "+ rangegroup.Name);
//					
//					foreach (CountryCode countrycode in rangegroup.CountryCodes)
//					{
//						foreach (string dialcode in countrycode.DialCodes)
//						{
//							Console.WriteLine ("\tDialcode: "+ dialcode);
//						}	
//					}
//				}
				
				Environment.Exit (0);
				
				List<Range> ranges = Range.List ();
				
				foreach (CountryCode countrycode in CountryCode.List ())
				{
					RangeGroup rangegroup = new RangeGroup ();
					rangegroup.Name = countrycode.Name;
					rangegroup.CountryCodes.Add (countrycode);
					
					Console.WriteLine ("Country: "+ countrycode.Name);
					foreach (string code in countrycode.DialCodes)
					{
						Console.WriteLine ("\t Code: "+ code);
						foreach (Range range in ranges)
						{					
							Match matchrange = Regex.Match (range.DialCode, @"^0*" + code);
							if (matchrange.Success) 
							{
								rangegroup.Ranges.Add (range);
								Console.WriteLine ("\t\t Name: "+ range.Name +" "+ range.DialCode);
							}
						}														
					}					
					
					rangegroup.Save ();
				}
				
//				foreach (Range range in Range.List ())
//				{
//					
//					Match matchrange = Regex.Match (range.DialCode, @"^0*" + "45");
//					if (matchrange.Success) 
//					{
//						Console.WriteLine ("Name: "+ range.Name);
//					}
					
//					rangegroup1.AddRange (range);
////					rangegroup1
////					rangegroup1.Ranges.Add (range);
//					Console.WriteLine ("Name: "+ range.Name);
////					
//				}				
//				
//				rangegroup1.Save ();
				
//				foreach (RangeGroup rangegroup in RangeGroup.List ())
//				{
//					Console.WriteLine ("RangeGroup: "+ rangegroup.Name);
//					
//					foreach (Range range in rangegroup.Ranges)
//					{
//						Console.WriteLine ("\tRange: "+ range.Name);
//						Console.WriteLine ("\t\t Dialcode: "+ range.DialCode);
//						Console.WriteLine ("\t\t Cost: ");
//						foreach (RangePrice rangeprice in range.Cost)
//						{
//							Console.WriteLine ("\t\t\t Price: "+ rangeprice.Price);
//							Console.WriteLine ("\t\t\t HourSpanBegin: "+ rangeprice.HourSpanBegin);
//							Console.WriteLine ("\t\t\t HourSpanEnd: "+ rangeprice.HourSpanEnd);
//							Console.WriteLine ("\t\t\t Weekdays: "+ rangeprice.Weekdays);
//							Console.WriteLine ("");
//						}
//					
//						Console.WriteLine ("\t\t Retail: ");
//						foreach (RangePrice rangeprice in range.Retail)
//						{
//							Console.WriteLine ("\t\t\t Price: "+ rangeprice.Price);
//							Console.WriteLine ("\t\t\t HourSpanBegin: "+ rangeprice.HourSpanBegin);
//							Console.WriteLine ("\t\t\t HourSpanEnd: "+ rangeprice.HourSpanEnd);
//							Console.WriteLine ("\t\t\t Weekdays: "+ rangeprice.Weekdays);
//							Console.WriteLine ("");
//						}					
//											
//						Range.Delete (range.Id);
//					}					
//					
//					RangeGroup.Delete (rangegroup.Id);
//				}				
				
//				Environment.Exit (0);
//				
//				Customer customer1 = new Customer ();
//				customer1.Name = "Test customer #1";				
//				customer1.Save ();
//			
//				Subscription subscription1 = new Subscription (customer1);
//				subscription1.Save ();							
//				
//				SIPAccount sipaccount1 = new SIPAccount (subscription1);
//				sipaccount1.Numbers.Add ("11111111");
//				sipaccount1.Numbers.Add ("22222222");
//				sipaccount1.Numbers.Add ("33333333");
//				sipaccount1.Save ();
//				
//				SIPAccount sipaccount2 = new SIPAccount (subscription1);
//				sipaccount2.Numbers.Add ("44444444");
//				sipaccount2.Numbers.Add ("55555555");
//				sipaccount2.Numbers.Add ("66666666");
//				sipaccount2.Save ();										
//				
//				foreach (Customer customer in Customer.List ())
//				{
//					foreach (Subscription subscription in customer.Subscriptions)
//					{
//						Console.WriteLine ("Subscription: "+ subscription.Id);
//						
//						foreach (SIPAccount sipaccount in subscription.SIPAccounts)
//						{
//							Console.WriteLine ("\t SIPAccount: "+ sipaccount.Id);
//							foreach (string number in sipaccount.Numbers)
//							{
//								Console.WriteLine ("\t\t Number: "+ number);
//							}
//						}
//					}
//					
//					Customer.Delete (customer.Id);
//				}		
			}	
			else
			{
				Console.WriteLine ("Could not connect to database.");
			}		
		}
	}
}

