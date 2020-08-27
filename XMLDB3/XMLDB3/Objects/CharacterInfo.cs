using System.Collections.Generic;

namespace XMLDB3
{
	public class CharacterInfo : Character
	{
		public Dictionary<long, Item> inventory;

		public string strToHash = string.Empty;

		public Item[] _inventory
		{
			get
			{
				if (inventory != null)
				{
					Item[] array = new Item[inventory.Values.Count];
					inventory.Values.CopyTo(array, 0);
					return array;
				}
				return null;
			}
			set
			{
				inventory = new Dictionary<long, Item>(value.Length);
				foreach (Item item in value)
				{
					inventory.Add(item.id, item);
				}
			}
		}

		public override string ToString()
		{
			return base.ToString() + ":" + id;
		}
	}
}
