using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Domain
{
	/// <summary>
	/// Model for store locations
	/// </summary>
	public class StoreLocation
	{
		public int StoreLocationId { get; set; } //PRIMARY KEY
		public virtual ICollection<StoreItem> StoreItems { get; set; } //RELATION TO STOREITEM
		private string _Location;
		public string Location // name of location
		{
			get { return _Location; }
			set { _Location = value; }
		}
		public static implicit operator StoreLocation(int v)
		{
			throw new NotImplementedException();
		}
	}
}
