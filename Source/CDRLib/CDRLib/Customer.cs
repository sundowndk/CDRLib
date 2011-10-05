using System;
using System.Collections.Generic;

using Toolbox.DBI;

namespace CDRLib
{
	public class Customer
	{
		#region Public Static Fields
		public static string DatabaseTableName = "customers";	
		#endregion
		
		#region Private Fields
		private Guid _id;
		private int _createtimestamp;
		private int _updatetimestamp;
		private string _name;
		private List<Guid> _subscriptionids;
		
		private string _subscriptionidsasstring
		{
			get
			{
				string result = string.Empty;
				if (this.__subscriptions == null)
				{
					foreach (Guid id in this._subscriptionids)
					{
						result += id.ToString () +";";
					}		
				}
				else
				{
					foreach (Subscription subscription in this.__subscriptions)
					{
						result += subscription.Id.ToString () +";";
					}
				}
				return result;
			}
			
			set
			{
				this.__subscriptions = null;
				this._subscriptionids.Clear ();
				foreach (string id in value.Split (";".ToCharArray (), StringSplitOptions.RemoveEmptyEntries))
				{
					this._subscriptionids.Add (new Guid (id));
				}
			}
		}
		#endregion
		
		#region Temp Fields
		private List<Subscription> __subscriptions;
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
		
		/// <summary>
		/// List of all <see cref="CDRLib.Subscription"/> instances belonging to this instance.
		/// </summary>
		public List<Subscription> Subscriptions
		{
			get
			{				
				if (this.__subscriptions == null)
				{
					this.__subscriptions = new List<Subscription> ();
					foreach (Guid id in this._subscriptionids)
					{
						this.__subscriptions.Add (Subscription.Load (id));					
					}
				}
				
				return this.__subscriptions;				
			}
		}		
		#endregion
		
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="CDRLib.Customer"/> class.
		/// </summary>
		public Customer ()
		{
			this._id = Guid.NewGuid ();
			this._createtimestamp = Toolbox.Date.CurrentDateTimeToTimestamp ();
			this._updatetimestamp = Toolbox.Date.CurrentDateTimeToTimestamp ();
			this._name = string.Empty;			
			this._subscriptionids = new List<Guid> ();
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
			qb.Columns (
				"id", 
				"createtimestamp", 
				"updatetimestamp", 
				"name",
				"subscriptionids"
				);
			
			qb.Values (	
				this._id, 
				this._createtimestamp, 
				this._updatetimestamp, 					
				this._name,
				this._subscriptionidsasstring
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
				throw new Exception (string.Format (Strings.Exception.CustomerSave, this._id));
			}		
		}
		#endregion
		
		#region Public Static Methods
		/// <summary>
		/// Load a <see cref="CDRLib.Customer"/> instance from database using a <see cref="System.Guid"/> identifier.
		/// </summary>
		public static Customer Load (Guid Id)
		{
			bool success = false;
			Customer result = new Customer ();

			QueryBuilder qb = new QueryBuilder (QueryBuilderType.Select);
			qb.Table (DatabaseTableName);
			qb.Columns (
				"id",
				"createtimestamp",
				"updatetimestamp",
				"name",
				"subscriptionids"
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
					result._subscriptionidsasstring = query.GetString (qb.ColumnPos ("subscriptionids"));

					success = true;
				}
			}

			query.Dispose ();
			query = null;
			qb = null;

			if (!success)
			{
				throw new Exception (string.Format (Strings.Exception.CustomerLoad, Id));
			}

			return result;
		}
		
		/// <summary>
		/// Delete a <see cref="CDRLib.Customer"/> instance from database using a <see cref="System.Guid"/> identifier.
		/// </summary>
		public static void Delete (Guid Id)
		{
			bool success = false;
			
			foreach (Subscription subscription in Customer.Load (Id).Subscriptions)
			{
				Subscription.Delete (subscription.Id);
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
				throw new Exception (string.Format (Strings.Exception.CustomerDelete, Id));
			}
		}		
		
		/// <summary>
		/// Returns a list of all <see cref="CDRLib.Customer"/> instances in the database.
		/// </summary>
		public static List<Customer> List ()
		{
			List<Customer> result = new List<Customer> ();
			
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

