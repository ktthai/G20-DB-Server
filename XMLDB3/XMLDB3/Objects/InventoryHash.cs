using System;

namespace XMLDB3
{
	public class InventoryHash
	{
		private string strToHash;

		private ulong hashToCompute;

		public InventoryHash(string _id)
		{
			strToHash = _id;
		}

		public InventoryHash(long _id)
		{
			ulong num = (ulong)_id;
			strToHash = num.ToString();
		}

		public void Add(Item _item)
		{
			hashToCompute += (uint)(_item.id & uint.MaxValue);
			hashToCompute += (uint)_item.Class;
		}

		public void Remove(Item _item)
		{
			if (hashToCompute != 0)
			{
				hashToCompute -= (uint)(_item.id & uint.MaxValue);
				hashToCompute -= (uint)_item.Class;
			}
		}

		public void Parse(string _strHash)
		{
			int num = _strHash.IndexOf(":", 0);
			if (num != -1)
			{
				hashToCompute = Convert.ToUInt64(_strHash.Substring(num + 1));
			}
		}

		public override string ToString()
		{
			return $"{strToHash}:{hashToCompute}";
		}

		public string Compute()
		{
			return InventoryHashUtility.ComputeHash(ToString());
		}

		public string ComputeInitial()
		{
			return InventoryHashUtility.ComputeInitialHash(strToHash);
		}
	}
}
