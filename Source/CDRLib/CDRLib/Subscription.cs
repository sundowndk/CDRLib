using System;
using System.Collections.Generic;

using Toolbox.DBI;

namespace CDRLib
{
	public class Subscription
	{
		#region Public Static Fields
		public static string DatabaseTableName = "subscriptions";	
		#endregion
		
		#region Private Fields
		private Guid _id;
		private int _createtimestamp;
		private int _updatetimestamp;		
		#endregion
		
		#region Public Fields
		/// <summary>
		/// <see cref="System.Guid"/> identifer for the instance.		
		/// </summary>		
		public Guid Id
		{
			get
			{
				return this._id;
			}
		}
		
		/// <summary>
		/// Timestamp from when the instance was created.
		/// </summary>		
		public int CreateTimestamp 
		{
			get 
			{ 
				return this._createtimestamp; 
			}
		}
		
		/// <summary>
		/// Timestamp from when the instance was last saved to the database.
		/// </summary>		
		public int UpdateTimestamp 
		{
			get 
			{ 
				return this._updatetimestamp; 
			}
		}		

		/// <summary>
		/// List of <see cref="CDRLib.SIPAccount"/> instances that belongs to the instance.
		/// </summary>
		public List<SIPAccount> SIPAccounts
		{
			get
			{
				return SIPAccount.List (this);				
			}
		}		
		#endregion
		
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="CDRLib.Subscription"/> class.
		/// </summary>
		public Subscription (Customer Customer)
		{
			this._id = Guid.NewGuid ();
			this._createtimestamp = Toolbox.Date.CurrentDateTimeToTimestamp ();
			this._updatetimestamp = Toolbox.Date.CurrentDateTimeToTimestamp ();						
		}
			
		private Subscription ()
		{			
		}
		#endregion
		
		#region Public Methods
		/// <summary>
		/// Save instance to database.
		/// </summary>		
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
			qb.Columns ("id", 
						"createtimestamp", 
						"updatetimestamp");
			
			qb.Values (	this._id, 
						this._createtimestamp, 
						this._updatetimestamp);
			
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
				throw new Exception (string.Format (Strings.Exception.SubscriptionSave, this._id));
			}		
		}
		#endregion
		
		#region Public Static Methods
		/// <summary>
		/// Load a <see cref="CDRLib.Subscription"/> instance from database using a <see cref="System.Guid"/> identifier.
		/// </summary>
		public static Subscription Load (Guid Id)
		{
			bool success = false;
			Subscription result = new Subscription ();

			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns ("id",
			            "createtimestamp",
			            "updatetimestamp");

			qb.AddWhere ("id", "=", Id);

			Query query = Runtime.DBConnection.Query (qb.QueryString);

			if (query.Success)
			{
				if (query.NextRow ())
				{
					result._id = query.GetGuid (qb.ColumnPos ("id"));
					result._createtimestamp = query.GetInt (qb.ColumnPos ("createtimestamp"));
					result._updatetimestamp = query.GetInt (qb.ColumnPos ("updatetimestamp"));						

					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
				throw new Exception (string.Format (Strings.Exception.SubscriptionLoad, Id));
			}

			return result;
		}
		
		/// <summary>
		/// Delete a <see cref="CDRLib.Subscription"/> instance from database using a <see cref="System.Guid"/> identifier.
		/// </summary>		
		public static void Delete (Guid Id)
		{
			bool success = false;
						
			foreach (SIPAccount sipaccount in Subscription.Load (Id).SIPAccounts)
			{
				SIPAccount.Delete (sipaccount.Id);
			}
						
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
				throw new Exception (string.Format (Strings.Exception.SubscriptionDelete, Id));
			}
		}	
				
		/// <summary>
		/// Returns a list of all <see cref="CDRLib.Subscription"/> instances in the database, belonging to a <see cref="CDRLib.Customer"/> instance.
		/// </summary>		
		internal static List<Subscription> List ()
		{
			List<Subscription> result = new List<Subscription> ();
			
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

