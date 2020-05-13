using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Domain
{
	/// <summary>
	/// Model for item inventories
	/// </summary>
	public class StoreItemInventory
	{
		public int StoreItemInventoryId { get; set; } //PRIMARY KEY
		public virtual List<StoreItem> StoreItem { get; set; }//RELATION TO STOREITEM

		private int _itemInventory; //inventory of item

		public int itemInventory
		{
			get { return _itemInventory; }
			set { _itemInventory = value; }
		}

	}
}
