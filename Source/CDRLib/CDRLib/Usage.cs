using System;
using System.Collections.Generic;

using Toolbox.DBI;

namespace CDRLib
{
	public class Usage
	{
		#region Public Static Fields
		public static string DatabaseTableName = "usages";
		#endregion
		
		#region Private Fields
		private Guid _id;
		private int _createtimestamp;
		private int _updatetimestamp;
		private Guid _subscriptionid;		
		private int _begintimestamp;
		private int _duration;
		private string _anumber;
		private string _bnumber;
		private Enums.UsageDirection _direction;
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
		
		public Guid SubscriptionId
		{
			get
			{
				return this._subscriptionid;
			}
		}
			
		public int BeginTimestamp
		{
			get
			{
				return this._begintimestamp;
			}
			
			set
			{
				this._begintimestamp = value;
			}
		}
		
		public string ANumber
		{
			get
			{
				return this._anumber;
			}
			
			set
			{
				this._anumber = value;
			}
		}

		public string BNumber
		{
			get
			{
				return this._bnumber;
			}
			
			set
			{
				this._bnumber = value;
			}
		}
		
		public int Duration
		{
			get
			{
				return this._duration;
			}
			
			set
			{
				this._duration = value;
			}
		}		
		
		public Enums.UsageDirection Direction
		{
			get
			{
				return this._direction;
			}
			
			set
			{
				this._direction = value;
			}
		}
		#endregion
		
		#region Constructor
		public Usage (Subscription Subscription)
		{
			this._id = Guid.NewGuid ();
			this._createtimestamp = Toolbox.Date.CurrentDateTimeToTimestamp ();
			this._updatetimestamp = Toolbox.Date.CurrentDateTimeToTimestamp ();		
			this._subscriptionid = Subscription.Id;
			this._begintimestamp = 0;
			this._duration = 0;
			this._anumber = string.Empty;
			this._bnumber = string.Empty;
			this._direction = CDRLib.Enums.UsageDirection.None;
		}
		
		private Usage ()
		{			
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
				"subscriptionid",
				"begintimestamp",
				"duration",
				"anumber",
				"bnumber",
				"direction"
				);
			
			qb.Values (	
				this._id, 
				this._createtimestamp, 
				this._updatetimestamp,
				this._subscriptionid,
				this._begintimestamp,
				this._duration,
				this._anumber,
				this._bnumber,
				this._direction
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
				throw new Exception (string.Format (Strings.Exception.UsageSave, this._id));
			}
		}		
		#endregion
		
		#region Public Static Methods
		public static Usage Load (Guid Id)
		{
			bool success = false;
			Usage result = new Usage ();

			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns (
				"id",
				"createtimestamp",
				"updatetimestamp",
				"subscriptionid",
				"begintimestamp",
				"duration",
				"anumber",
				"bnumber",
				"direction"
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
					result._subscriptionid = query.GetGuid (qb.ColumnPos ("subscriptionid"));
					result._begintimestamp = query.GetInt (qb.ColumnPos ("begintimestamp"));	
					result._duration = query.GetInt (qb.ColumnPos ("duration"));	
					result._anumber = query.GetString (qb.ColumnPos ("anumber"));
					result._bnumber = query.GetString (qb.ColumnPos ("anumber"));
					result._direction = query.GetEnum<Enums.UsageDirection> (qb.ColumnPos ("direction"));
					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
				throw new Exception (string.Format (Strings.Exception.UsageLoad, Id));
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
				throw new Exception (string.Format (Strings.Exception.UsageDelete, Id));
			}
		}	
		
		public static List<Usage> List ()
		{
			List<Usage> result = new List<Usage> ();
			
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

