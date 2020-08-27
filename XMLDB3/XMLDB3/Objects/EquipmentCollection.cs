using System;
using System.Collections.Generic;

namespace XMLDB3
{
	
	public class EquipmentCollection : EquipmentCollectionBase
	{
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

		public EquipmentCollection()
		{
            info = null;
            item = new List<CollectionItem>();
        }

		public EquipmentCollection(CollectionInfo _info, List<CollectionItem> _item)
		{
			info = _info;
			item = _item;
		}

		public bool IsValid()
		{
			if (info == null)
			{
				return false;
			}
			if (info.account == null || info.account == string.Empty)
			{
				return false;
			}
			return true;
		}
	}
}
