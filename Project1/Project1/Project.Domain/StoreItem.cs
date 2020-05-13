using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project1.Domain
{
	/// <summary>
	/// Model for store items in each locations
	/// </summary>
	public class StoreItem
	{
		public int StoreItemId { get; set; } //PRIMARY KEY
		public virtual StoreLocation StoreLocation { get; set; } //RELATION TO STORELOCATION
		public virtual StoreItemInventory StoreItemInventory { get; set; } //RELATION TO STOREITEMINVENTORY

		private string _itemName; //name of the item

		public string itemName
		{
			get { return _itemName; }
			set { _itemName = value; }
		}
		private double _itemPrice; //price of the item

		public double itemPrice
		{
			get { return _itemPrice; }
			set { _itemPrice = value; }
		}
	}
}
