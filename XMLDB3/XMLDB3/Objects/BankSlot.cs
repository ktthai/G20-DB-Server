using System;
using System.Linq;

namespace XMLDB3
{
	public class BankSlot : BankSlotBase
	{
		public string Name => slot.name;

		public BankRace Race => (BankRace)slot.race;

		public BankSlot()
		{
			slot = null;
			item = new System.Collections.Generic.List<BankItem>();
		}

		public BankSlot(BankSlotInfo _info, BankItem[] _item)
		{
			slot = _info;
			if (_item != null)
				item = _item.ToList();
			else
				item = new System.Collections.Generic.List<BankItem>();
		}

		public BankSlot(string _name, BankRace _race)
		{
			if (_name == null || _name.Length == 0)
			{
				throw new Exception("Bank slot name is null or empty");
			}
			slot = new BankSlotInfo();
			slot.name = _name;
			slot.race = (byte)_race;
			slot.AdvancedItemReceiveTime = 0;
            item = new System.Collections.Generic.List<BankItem>();
        }

		public bool IsValid()
		{
			if (slot == null)
			{
				return false;
			}
			if (slot.name == null || slot.name == string.Empty)
			{
				return false;
			}
			return true;
		}
	}
}
