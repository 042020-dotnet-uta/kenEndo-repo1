using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using Project1.Domain;

namespace Project1.Models
{
	/// <summary>
	/// Model for store items in each locations
	/// </summary>
	public class StoreItemModel
	{
		public int quantity { get; set; }
		public List<StoreItem> storeItems { get; set; }
	}
}
