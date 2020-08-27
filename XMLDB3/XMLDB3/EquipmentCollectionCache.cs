using System;
using System.Collections;
using System.Collections.Generic;

namespace XMLDB3
{
	public class EquipmentCollectionCache
	{
		private CollectionInfo info;

		private Hashtable inventory;

		public string Account
		{
			get
			{
				if (info == null)
				{
					return null;
				}
				return info.account;
			}
			set
			{
				if (value != null && string.Empty != value)
				{
					if (info == null)
					{
						info = new CollectionInfo();
					}
					info.account = value;
				}
			}
		}

		public DateTime UpdateTime
		{
			get
			{
				if (info != null)
				{
					return info.updateTime;
				}
				return DateTime.MinValue;
			}
			set
			{
				if (info != null)
				{
					info.updateTime = value;
				}
			}
		}

		public Hashtable Inventory => inventory;

		public EquipmentCollectionCache()
		{
			info = null;
			inventory = null;
		}

		public EquipmentCollectionCache(EquipmentCollection _equipmentCollection)
		{
			info = null;
			inventory = null;
			Update(_equipmentCollection);
		}

		public void Update(EquipmentCollection _equipmentCollection)
		{
			if (_equipmentCollection == null || !_equipmentCollection.IsValid())
			{
				throw new ArgumentException("유효하지 않은 의상 수집 시스템 데이터입니다.", "_data");
			}
			info = _equipmentCollection.info;
			inventory = null;
			InsertItemToInventory(_equipmentCollection.item);
		}

		private void InsertItemToInventory(List<CollectionItem> _collectItem)
		{
			if (inventory == null)
			{
				inventory = new Hashtable();
			}
			if (!IsValid())
			{
				throw new ArgumentException("의상 수집 시스템 캐시가 유효하지 않습니다.", "_data");
			}
			if (info.account != null && _collectItem != null)
			{
				foreach (CollectionItem collectionItem in _collectItem)
				{
					if (collectionItem != null)
					{
						inventory.Add(collectionItem.item.id, collectionItem);
					}
				}
			}
			else
			{
				inventory = null;
			}
		}

		public bool IsValid()
		{
			if (info != null && info.account != null)
			{
				return info.account != string.Empty;
			}
			return false;
		}

		public void Invalidate()
		{
			info = null;
			inventory = null;
		}

		public EquipmentCollection ToEquipmentCollection()
		{
			if (IsValid())
			{
				List<CollectionItem> array = null;
				if (inventory != null && inventory.Count > 0)
				{
					array = new List<CollectionItem>(inventory.Count);
					int num = 0;
					foreach (CollectionItem value in inventory.Values)
					{
						if (value != null && num < inventory.Count)
						{
							array[num++] = value;
						}
					}
				}
				return new EquipmentCollection(info, array);
			}
			return null;
		}

		public void AddItem(CollectionItem _item)
		{
			if (inventory != null)
			{
				inventory.Add(_item.item.id, _item);
			}
		}

		public CollectionItem FindItem(long _key)
		{
			if (inventory == null)
			{
				return null;
			}
			if (inventory.ContainsKey(_key))
			{
				return (CollectionItem)inventory[_key];
			}
			return null;
		}

		public CollectionItem FindItem(CollectionItem _item)
		{
			if (_item == null)
			{
				return null;
			}
			return FindItem(_item.item.id);
		}

		public void RemoveItem(long _key)
		{
			if (inventory != null)
			{
				inventory.Remove(_key);
			}
		}

		public void RemoveItem(CollectionItem _item)
		{
			RemoveItem(_item.item.id);
		}
	}
}
