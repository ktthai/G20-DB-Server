using System.Collections.Generic;

namespace XMLDB3
{
    public class Bank : BankBase
	{
        
        public const byte MaxBankRace = 3;

		private bool[] raceLoadState;

		public List<BankSlot> slot;

		private int[] wealth;

		public int humanWealth
		{
			get
			{
				return GetWealth(BankRace.Human);
			}
			set
			{
				SetWealth(BankRace.Human, value);
			}
		}

		public int elfWealth
		{
			get
			{
				return GetWealth(BankRace.Elf);
			}
			set
			{
				SetWealth(BankRace.Elf, value);
			}
		}

		public int giantWealth
		{
			get
			{
				return GetWealth(BankRace.Giant);
			}
			set
			{
				SetWealth(BankRace.Giant, value);
			}
		}

		public Bank()
		{
			slot = new List<BankSlot>();
			raceLoadState = new bool[3];
			wealth = new int[3];
		}

		public bool IsValid()
		{
			if (account == null || account == string.Empty)
			{
				return false;
			}
			if (data == null)
			{
				return false;
			}
			return true;
		}

		public bool IsBankLoaded(BankRace _race)
		{
			if (_race != BankRace.None)
			{
				return raceLoadState[(uint)_race];
			}
			return IsValid();
		}

		public void SetBankLoadState(BankRace _race)
		{
			if (3 > (int)_race)
			{
				raceLoadState[(uint)_race] = true;
			}
		}

		public void SetBankLoadStateAll(bool _bLoad)
		{
			for (int i = 0; i < 3; i++)
			{
				raceLoadState[i] = _bLoad;
			}
		}

		public void SetWealth(BankRace _race, int _value)
		{
			if (3 > (int)_race)
			{
				wealth[(uint)_race] = _value;
			}
		}

		public int GetWealth(BankRace _race)
		{
			if (3 <= (int)_race)
			{
				return 0;
			}
			return wealth[(uint)_race];
		}
	}
}
