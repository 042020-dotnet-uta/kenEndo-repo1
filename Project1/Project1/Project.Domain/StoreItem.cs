using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Project1.Domain
{
	/// <summary>
	/// Model for store items in each locations
	/// </summary>
	public class StoreItem
	{
		public int StoreItemId { get; set; } //PRIMARY KEY
		//[JsonIgnore]
		//[IgnoreDataMember]
		public virtual StoreLocation StoreLocation { get; set; } //RELATION TO STORELOCATION
		public virtual StoreItemInventory StoreItemInventory { get; set; } //RELATION TO STOREITEMINVENTORY

		private string _itemName; //name of the item
		[DisplayName("Pet Name")]
		public string itemName
		{
			get { return _itemName; }
			set { _itemName = value; }
		}
		private double _itemPrice; //price of the item
		[DisplayName("Pet Price")]
		public double itemPrice
		{
			get { return _itemPrice; }
			set { _itemPrice = value; }
		}
	}
}
