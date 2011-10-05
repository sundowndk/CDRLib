using System;

namespace CDRLib.Strings
{
	public class Exception
	{
		#region CUSTOMER
		public static string CustomerLoad = "CUSTOMER with id: {0} was not found.";
		public static string CustomerSave = "Could not save CUSTOMER with id: {0}";
		public static string CustomerDelete = "Could not delete CUSTOMER with id: {0}";
		#endregion
		
		#region SUBSCRIPTION
		public static string SubscriptionLoad = "SUBSCRIPTION with id: {0} was not found.";
		public static string SubscriptionSave = "Could not save SUBSCRIPTION with id: {0}";
		public static string SubscriptionDelete = "Could not delete SUBSCRIPTION with id: {0}";
		#endregion
		
		#region SIPACCOUNT
		public static string SIPAccountLoad = "SIPACCOUNT with id: {0} was not found.";
		public static string SIPAccountSave = "Could not save SIPACCOUNT with id: {0}";
		public static string SIPAccountDelete = "Could not delete SIPACCOUNT with id: {0}";
		#endregion		
		
		#region RANGE
		public static string RangeLoad = "RANGE with id: {0} was not found.";
		public static string RangeSave = "Could not save RANGE with id: {0}";
		public static string RangeDelete = "Could not delete RANGE with id: {0}";		
		#endregion
		
		#region RANGEGROUP
		public static string RangeGroupLoad = "RANGEGROUP with id: {0} was not found.";
		public static string RangeGroupSave = "Could not save RANGEGROUP with id: {0}";
		public static string RangeGroupDelete = "Could not delete RANGEGROUP with id: {0}";		
		#endregion		
		
		#region COUNTRYCODE
		public static string CountryCodeLoad = "COUNTRYCODE with id: {0} was not found.";
		public static string CountryCodeSave = "Could not save COUNTRYCODE with id: {0}";
		public static string CountryCodeDelete = "Could not delete COUNTRYCODE with id: {0}";		
		#endregion			
		
		#region RANGEPRICE
		public static string RangePriceLoad = "RANGEPRICE with id: {0} was not found.";
		public static string RangePriceSave = "Could not save RANGEPRICE with id: {0}";
		public static string RangePriceDelete = "Could not delete RANGEPRICE with id: {0}";		
		#endregion			

		#region RANGEPRICEGROUP
		public static string RangePriceGroupLoad = "RANGEPRICEGROUP with id: {0} was not found.";
		public static string RangePriceGroupSave = "Could not save RANGEPRICEGROUP with id: {0}";
		public static string RangePriceGroupDelete = "Could not delete RANGEPRICEGROUP with id: {0}";		
		#endregion			
		
		#region USAGE
		public static string UsageLoad = "USAGE with id: {0} was not found.";
		public static string UsageSave = "Could not save USAGE with id: {0}";
		public static string UsageDelete = "Could not delete USAGE with id: {0}";		
		#endregion			
	}
}
