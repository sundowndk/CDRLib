using System;
using System.Collections.Generic;

using Toolbox.DBI;

namespace CDRLib
{
	public class RangePriceGroup
	{
		#region Public Static Fields
		public static string DatabaseTableName = "rangepricegroups";
		#endregion
		
		#region Private Fields
		private Guid _id;
		private int _createtimestamp;
		private int _updatetimestamp;		
		private string _name;
		private List<Guid> _rangepriceids;			
		private int _validfromtimestamp;
		private int _validtotimestamp;
		
		private string _rangepriceidsasstring
		{
			get
			{
				string result = string.Empty;				
				if (this.__rangeprices == null)
				{
					foreach (Guid id in this._rangepriceids)
					{
						result += id.ToString () +";";
					}
				}
				else
				{
					foreach (RangePrice rangeprice in this.__rangeprices)
					{
						result += rangeprice.Id.ToString () +";";
					}
				}
				return result;
			}
			
			set
			{
				this.__rangeprices = null;
				this._rangepriceids.Clear ();
				foreach (string id in value.Split (";".ToCharArray (), StringSplitOptions.RemoveEmptyEntries))
				{
					this._rangepriceids.Add (new Guid (id));
				}
			}
		}
		#endregion
		
		#region Temp Fields
		private List<RangePrice> __rangeprices;
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
				
		public int ValidFromTimestamp
		{
			get
			{
				return this._validfromtimestamp;
			}
			
			set
			{
				this._validfromtimestamp = value;
			}
		}
		
		public int ValidToTimestamp
		{
			get
			{
				return this._validtotimestamp;
			}
			
			set
			{
				this._validtotimestamp = value;
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
		
		public List<RangePrice> RangePrices
		{
			get
			{
				if (this.__rangeprices == null)
				{
					this.__rangeprices = new List<RangePrice> ();
					foreach (Guid id in this._rangepriceids)
					{
						this.__rangeprices.Add (RangePrice.Load (id));
					}					
				}
				return this.__rangeprices;
			}
		}		
		#endregion
			
		#region Constructor
		public RangePriceGroup ()
		{
			this._id = Guid.NewGuid ();
			this._createtimestamp = Toolbox.Date.CurrentDateTimeToTimestamp ();
			this._updatetimestamp = Toolbox.Date.CurrentDateTimeToTimestamp ();	
			this._validfromtimestamp = 0;
			this._validtotimestamp = 0;
			this._name = string.Empty;
			this._rangepriceids = new List<Guid> ();
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
				"validfromtimestamp",
				"validtotimestamp",
				"name",
				"rangepriceids"				
				);
			
			qb.Values (	
				this._id, 
				this._createtimestamp, 
				this._updatetimestamp,			
				this._validfromtimestamp,
				this._validtotimestamp,
				this._name,
				this._rangepriceidsasstring
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
				throw new Exception (string.Format (Strings.Exception.RangePriceGroupSave, this._id));
			}
		}
		#endregion		
		
		#region Public Static Methods
		public static RangePriceGroup Load (Guid Id)
		{
			bool success = false;
			RangePriceGroup result = new RangePriceGroup ();

			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns (
				"id",
				"createtimestamp",
				"updatetimestamp",				
				"validfromtimestamp",
				"validtotimestamp",
				"name",
				"rangepriceids"
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
					result._validfromtimestamp = query.GetInt (qb.ColumnPos ("validfromtimestamp"));	
					result._validtotimestamp  = query.GetInt (qb.ColumnPos ("validtotimestamp"));	
					result._name = query.GetString (qb.ColumnPos ("name"));		
					result._rangepriceidsasstring = query.GetString (qb.ColumnPos ("rangepricegroupids"));
					
					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
				throw new Exception (string.Format (Strings.Exception.RangePriceGroupLoad, Id));
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
				throw new Exception (string.Format (Strings.Exception.RangePriceGroupDelete, Id));
			}
		}	
		
		public static List<RangePriceGroup> List (Range Range)
		{
			List<RangePriceGroup> result = new List<RangePriceGroup> ();
			
			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns ("id");
			qb.AddWhere ("rangeid", "=", Range.Id);
			
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

