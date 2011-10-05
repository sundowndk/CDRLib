using System;
using System.Collections.Generic;

using Toolbox;
using Toolbox.DBI;

namespace CDRLib
{
	public class RangePrice
	{
		#region Public Static Fields
		public static string DatabaseTableName = "rangeprices";
		#endregion		
		
		#region Private Fields
		private Guid _id;
		private int _createtimestamp;
		private int _updatetimestamp;
		private decimal _price;
		
		private string _hourspanbegin;
		private string _hourspanend;
		private Enums.Weekday _weekdays;
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
		
		public decimal Price
		{
			get
			{
				return this._price;
			}
			
			set
			{
				this._price = value;
			}
		}
		
		public string HourSpanBegin
		{
			get
			{
				return this._hourspanbegin;
			}
			
			set
			{
				this._hourspanbegin = value;
			}
		}
		
		public string HourSpanEnd
		{
			get
			{
				return this._hourspanend;
			}
			
			set
			{
				this._hourspanend = value;
			}
		}
		
		public Enums.Weekday Weekdays
		{
			get
			{
				return this._weekdays;
			}
			
			set
			{
				this._weekdays = value;
			}
		}				
		#endregion
		
		#region Constructor
		public RangePrice ()
		{
			this._id = Guid.NewGuid ();
			this._createtimestamp = Toolbox.Date.CurrentDateTimeToTimestamp ();
			this._updatetimestamp = Toolbox.Date.CurrentDateTimeToTimestamp ();
			this._price = 0;
			this._hourspanbegin = "00:00";
			this._hourspanend = "00:00";
			this._weekdays = CDRLib.Enums.Weekday.All;
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
				"price",
				"hourspanbegin",
				"hourspanend",
				"weekdays"
				);
			
			qb.Values (	
				this._id, 
				this._createtimestamp, 
				this._updatetimestamp,
				this._price,
				this._hourspanbegin,
				this._hourspanend,
				this._weekdays
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
				throw new Exception (string.Format (Strings.Exception.RangePriceSave, this._id));
			}
		}
		#endregion	
		
		#region Public Static Methods
		public static RangePrice Load (Guid Id)
		{
			bool success = false;
			RangePrice result = new RangePrice ();

			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns (
				"id",
				"createtimestamp",
				"updatetimestamp",
				"price",
				"hourspanbegin",
				"hourspanend",
				"weekdays"
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
					result._price = query.GetDecimal (qb.ColumnPos ("price"));		
					result._hourspanbegin = query.GetString (qb.ColumnPos ("hourspanbegin"));
					result._hourspanend = query.GetString (qb.ColumnPos ("hourspanend"));		
					result._weekdays = query.GetEnum<Enums.Weekday> (qb.ColumnPos ("weekdays"));
					
					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
				throw new Exception (string.Format (Strings.Exception.RangePriceLoad, Id));
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
				throw new Exception (string.Format (Strings.Exception.RangePriceDelete, Id));
			}
		}		
		#endregion
		
	}
}

