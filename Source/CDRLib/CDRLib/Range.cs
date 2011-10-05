using System;
using System.Collections.Generic;

using Toolbox;
using Toolbox.DBI;

namespace CDRLib
{
	public class Range
	{
		#region Public Static Fields
		public static string DatabaseTableName = "ranges";			
		#endregion
		
		#region Private Fields
		private Guid _id;
		private int _createtimestamp;
		private int _updatetimestamp;
		
		private List<RangePriceGroup> _costrangepricegroups;
		private List<RangePriceGroup> _retailrangepricegroups;
		
		private string _costrangepricegroupsasstring
		{
			get
			{
				string result = string.Empty;
				foreach (RangePriceGroup rangepricegroup in this._costrangepricegroups)
				{
					result += rangepricegroup.Id.ToString () +";";
				}
				return result;
			}
			
			set
			{
				this._costrangepricegroups.Clear ();
				foreach (string id in value.Split (";".ToCharArray (), StringSplitOptions.RemoveEmptyEntries))
				{
					try
					{
						this._costrangepricegroups.Add (RangePriceGroup.Load (new Guid (id)));
					}
					catch
					{}
				}
			}
		}
		
		private string _retailpricegroupsasstring
		{
			get
			{
				string result = string.Empty;
				foreach (RangePriceGroup rangepricegroup in this._retailrangepricegroups)
				{
					result += rangepricegroup.Id.ToString () +";";
				}
				return result;
			}
			
			set
			{
				this._retailrangepricegroups.Clear ();
				foreach (string id in value.Split (";".ToCharArray (), StringSplitOptions.RemoveEmptyEntries))
				{
					try
					{
						this._retailrangepricegroups.Add (RangePriceGroup.Load (new Guid (id)));
					}
					catch
					{}
				}
			}
		}
		
		private string _name;
		private string _dialcode;
		#endregion
		
		#region Public Fields
		public Guid Id
		{
			get
			{
				return this._id;
			}
		}		
		
		public int CreateTimestamp
		{
			get
			{
				return this._createtimestamp;
			}
		}
		
		public int UpdateTimestamp
		{
			get
			{
				return this._updatetimestamp;
			}
		}
		
		public string Name
		{
			get
			{
				return this._name;
			}
			
			set
			{
				this._name = value;
			}
		}
		
		public List<RangePriceGroup> CostRangePriceGroups
		{
			get
			{				
				return this._costrangepricegroups;
			}
		}
		
		public List<RangePriceGroup> RetailRangePriceGroups
		{
			get
			{
				return this._retailrangepricegroups;
			}
		}		
		
		public string DialCode
		{
			get
			{
				return this._dialcode;
			}
			
			set
			{
				this._dialcode = value;
			}
		}
		#endregion
		
		#region Constructor		
		public Range ()
		{
			this._id = Guid.NewGuid ();
			this._createtimestamp = Toolbox.Date.CurrentDateTimeToTimestamp ();
			this._updatetimestamp = Toolbox.Date.CurrentDateTimeToTimestamp ();
			this._costrangepricegroups = new List<RangePriceGroup> ();
			this._retailrangepricegroups = new List<RangePriceGroup> ();
			this._name = string.Empty;
			this._dialcode = string.Empty;
		}
		#endregion
		
		#region Public Methods
		public void Save ()
		{
			bool success = false;
			QueryBuilder qb = null;
			
			if (!Helpers.GuidExists (Runtime.DBConnection, DatabaseTableName, this._id)) 
			{
				qb = new QueryBuilder (QueryBuilderType.Insert);
			} 
			else 
			{
				qb = new QueryBuilder (QueryBuilderType.Update);
				qb.AddWhere ("id", "=", this._id);
			}
			
			this._updatetimestamp = Toolbox.Date.CurrentDateTimeToTimestamp ();
			
			qb.Table (DatabaseTableName);
			qb.Columns (
				"id", 
				"createtimestamp", 
				"updatetimestamp",
				"costrangepricegroupids",
				"retailrangepricegroupids",
				"name",
				"dialcode"
				);
			
			qb.Values (	
				this._id, 
				this._createtimestamp, 
				this._updatetimestamp,
				this._costrangepricegroupsasstring,
				this._retailpricegroupsasstring,
				this._name,				
				this._dialcode
				);
			
			Query query = Runtime.DBConnection.Query (qb.QueryString);
			
			if (query.AffectedRows > 0) 
			{
				success = true;
			}
			
			query.Dispose ();
			query = null;
			qb = null;
			
			if (!success) 
			{
				throw new Exception (string.Format (Strings.Exception.RangeSave, this._id));
			}		
		}		
		
		public decimal Cost (DateTime Date)
		{
			decimal result = 0;
			int timestamp = Toolbox.Date.DateTimeToTimestamp (Date);
			
			foreach (RangePriceGroup rangepricegroup in this._costrangepricegroups)
			{
				if (timestamp > rangepricegroup.ValidFromTimestamp && timestamp < rangepricegroup.ValidToTimestamp)
				{
					foreach (RangePrice rangeprice in rangepricegroup.RangePrices)
					{
						Console.WriteLine (Date.DayOfWeek);
					}
				}
			}
			
			return result;
		}
		
		public decimal Retail (DateTime Date)
		{
			decimal result = 0;
			
			
			
			return result;
		}
		#endregion

		#region Public Static Methods
		public static Range Load (Guid Id)
		{
			bool success = false;
			Range result = new Range ();

			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns (
				"id",
				"createtimestamp",
				"updatetimestamp",
				"costrangepricegroupids",
				"retailrangepricegroupids",
				"name",
				"dialcode"
				);

			qb.AddWhere ("id", "=", Id);

			Query query = Runtime.DBConnection.Query (qb.QueryString);

			if (query.Success)
			{
				if (query.NextRow ())
				{
					result._id = query.GetGuid (qb.ColumnPos ("id"));
					result._createtimestamp = query.GetInt (qb.ColumnPos ("createtimestamp"));
					result._updatetimestamp = query.GetInt (qb.ColumnPos ("updatetimestamp"));	
					result._costrangepricegroupsasstring = query.GetString (qb.ColumnPos ("costrangepricegroupids"));
					result._retailpricegroupsasstring = query.GetString (qb.ColumnPos ("retailrangepricegroupids"));
					result._name = query.GetString (qb.ColumnPos ("name"));															
					result._dialcode = query.GetString (qb.ColumnPos ("dialcode"));
					
					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
				throw new Exception (string.Format (Strings.Exception.RangeLoad, Id));
			}

			return result;
		}
		
		public static void Delete (Guid Id)
		{
			bool success = false;
			
			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Delete);
			qb.Table (DatabaseTableName);
			
			qb.AddWhere ("id", "=", Id);
			
			Query query = Runtime.DBConnection.Query (qb.QueryString);
			
			if (query.AffectedRows > 0) 
			{
				success = true;
			}
			
			query.Dispose ();
			query = null;
			qb = null;
			
			if (!success) 
			{
				throw new Exception (string.Format (Strings.Exception.RangeDelete, Id));
			}
		}	
		
		public static List<Range> List ()
		{
			List<Range> result = new List<Range> ();
			
			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns ("id");
			
			Query query = Runtime.DBConnection.Query (qb.QueryString);
			if (query.Success)
			{
				while (query.NextRow ())
				{					
					try
					{
						result.Add (Load (query.GetGuid (qb.ColumnPos ("id"))));
					}
					catch
					{}
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			return result;
		}		
		#endregion
	}
}

