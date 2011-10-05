using System;
using System.Collections;
using System.Collections.Generic;

using Toolbox.DBI;

namespace CDRLib
{
	public class RangeGroup
	{
		#region Public Static Fields
		public static string DatabaseTableName = "rangegroups";
		#endregion
		
		#region Private Fields
		private Guid _id;
		private int _createtimestamp;
		private int _updatetimestamp;
		private string _name;
		private List<string> _rangeids;
		private List<Guid> _countrycodeids;
		
		private string _countrycodeidsasstring
		{
			get
			{
				string result = string.Empty;
				if (this.__countrycodes != null)
				{
					List<CountryCode> temp1 = new List<CountryCode> ();
					List<Guid> temp2 = new List<Guid> ();
					
					foreach (string range in this._rangeids)
					{
						string[] split = range.Split (":".ToCharArray ());
						if (split[0] == "RANGEGROUP")
						{
							temp1.AddRange (RangeGroup.Load (new Guid (split[1])).CountryCodes);
						}
					}					
					
					foreach (CountryCode countrycode in temp1)
					{
						temp2.Add (countrycode.Id);
					}
					
					foreach (CountryCode countrycode in this.__countrycodes)
					{
						if (!temp2.Contains (countrycode.Id))
						{
							result += countrycode.Id.ToString () +";";
						}
					}
										
					foreach (Guid id in this._countrycodeids)
					{
						result += id.ToString () +";";
					}		
				}
				else
				{
					foreach (Guid id in this._countrycodeids)
					{
						result += Id.ToString () +";";
					}
				}
				return result;
			}
			
			set
			{
				this._countrycodeids.Clear ();
				foreach (string id in value.Split (";".ToCharArray (), StringSplitOptions.RemoveEmptyEntries))
				{
					this._countrycodeids.Add (new Guid (id));
				}
			}
		}
		
		private string _rangeidsasstring
		{
			get
			{
				string result = string.Empty;
				if (this.__ranges != null)
				{
					foreach (object range in this.__ranges)
					{
						if (range.GetType () == typeof (Range))
						{
							result += "RANGE:"+ ((Range)range).Id.ToString () +";";
						} 
						else if (range.GetType () == typeof (RangeGroup))
						{
							result += "RANGEGROUP:"+ ((RangeGroup)range).Id.ToString () +";";
						}
					}
				}
				else
				{
					foreach (string range in this._rangeids)
					{
						result += range +";";						
					}
				}
				
				return result;
			}
			
			set
			{
				this._rangeids.Clear ();				
				foreach (string data in value.Split (";".ToCharArray (), StringSplitOptions.RemoveEmptyEntries))
				{
					this._rangeids.Add (data);
				}			
			}
		}
		
		#endregion		
		
		#region Temp Fields
		private List<object> __ranges;
		private List<CountryCode> __countrycodes;
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
		
		public List<CountryCode> CountryCodes
		{
			get
			{
				if (this.__countrycodes == null)
				{
					this.__countrycodes = new List<CountryCode> ();
					foreach (Guid id in this._countrycodeids)
					{
						this.__countrycodes.Add (CountryCode.Load (id));
					}		
					
					foreach (string range in this._rangeids)
					{						
						string[] split = range.Split (":".ToCharArray ());
						if (split[0] == "RANGEGROUP")
						{
							this.__countrycodes.AddRange (RangeGroup.Load (new Guid (split[1])).CountryCodes);
						}
					}
				}
				
				return this.__countrycodes;				
			}
		}
		
		public List<object> Ranges 
		{
			get
			{
				if (this.__ranges == null)
				{
					this.__ranges = new List<object> ();										
					foreach (string range in this._rangeids)
					{
						string[] split = range.Split (":".ToCharArray ());
						if (split[0] == "RANGE")
						{
							this.__ranges.Add (Range.Load (new Guid (split[1])));
						}
						else if (split[0] == "RANGEGROUP")
						{
							this.__ranges.Add (RangeGroup.Load (new Guid (split[1])));
						}
					}										
				}
				
				return this.__ranges;
			}
		}
		#endregion
				
		#region Constructor
		public RangeGroup ()
		{
			this._id = Guid.NewGuid ();
			this._createtimestamp = Toolbox.Date.CurrentDateTimeToTimestamp ();
			this._updatetimestamp = Toolbox.Date.CurrentDateTimeToTimestamp ();
			this._name = string.Empty;
			this._countrycodeids = new List<Guid> ();
			this._rangeids = new List<string> ();				
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
				"name",
				"countrycodeids",
				"rangeids"
				);
			
			qb.Values (	
				this._id, 
				this._createtimestamp, 
				this._updatetimestamp,
				this._name,
				this._countrycodeidsasstring,
				this._rangeidsasstring
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
				throw new Exception (string.Format (Strings.Exception.RangeGroupSave, this._id));
			}
		}
		#endregion
		
		#region Public Static Methods
		public static RangeGroup Load (Guid Id)
		{
			bool success = false;
			RangeGroup result = new RangeGroup ();

			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns (
				"id",
				"createtimestamp",
				"updatetimestamp",
				"name",
				"countrycodeids",
				"rangeids"
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
					result._name = query.GetString (qb.ColumnPos ("name"));		
					result._countrycodeidsasstring = query.GetString (qb.ColumnPos ("countrycodeids"));
					result._rangeidsasstring =  query.GetString (qb.ColumnPos ("rangeids"));					
					
					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
				throw new Exception (string.Format (Strings.Exception.RangeGroupLoad, Id));
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
				throw new Exception (string.Format (Strings.Exception.RangeGroupDelete, Id));
			}
		}	
		
		public static List<RangeGroup> List ()
		{
			List<RangeGroup> result = new List<RangeGroup> ();
			
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

