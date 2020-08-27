using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace XMLDB3
{
	public class InventoryHashUtility
	{
		private static DateTime hashMissDate;

		static InventoryHashUtility()
		{
			hashMissDate = new DateTime(2011, 8, 1, 0, 0, 0, 0);
		}

		public static bool CheckHash(string _hash, string _strToHash, long _id, DateTime _updateTime)
		{
			string a = ComputeHash(_strToHash);
			if (a != _hash)
			{
				if (_updateTime.Ticks > hashMissDate.Ticks)
				{
					return false;
				}
				a = ComputeInitialHash(_id);
				if (a != _hash)
				{
					return false;
				}
			}
			return true;
		}

		public static bool CheckHash(string _hash, string _id, int _deposit, DateTime _updateTime)
		{
			string a = ComputeHash(_id, _deposit);
			if (a != _hash)
			{
				if (_updateTime.Ticks > hashMissDate.Ticks)
				{
					return false;
				}
				a = ComputeInitialHash(_id);
				if (a != _hash)
				{
					return false;
				}
			}
			return true;
		}

		public static bool CheckHash(string _hash, string _strToHash, string _id, DateTime _updateTime)
		{
			string a = ComputeHash(_strToHash);
			if (a != _hash)
			{
				if (_updateTime.Ticks > hashMissDate.Ticks)
				{
					return false;
				}
				a = ComputeInitialHash(_id);
				if (a != _hash)
				{
					return false;
				}
			}
			return true;
		}

		public static string ComputeHash(string _id, int _deposit)
		{
			uint num = (uint)_deposit;
			return ComputeHash($"{_id}#{num.ToString()};");
		}

		public static string ComputeStrToHash(long _id, List<Item> _items)
		{
			InventoryHash inventoryHash = new InventoryHash(_id);
			if (_items != null)
			{
				foreach (Item item in _items)
				{
					inventoryHash.Add(item);
				}
			}
			return inventoryHash.ToString();
		}

		public static string ComputeHash(long _id, List<Item> _items)
		{
			return ComputeHash(ComputeStrToHash(_id, _items));
		}

		public static string ComputeStrToHash(string _name, List<BankItem> _items)
		{
			InventoryHash inventoryHash = new InventoryHash(_name);
			if (_items != null)
			{
				foreach (BankItem bankItem in _items)
				{
					inventoryHash.Add(bankItem.item);
				}
			}
			return inventoryHash.ToString();
		}

		public static string ComputeHash(string _name, List<BankItem> _items)
		{
			return ComputeHash(ComputeStrToHash(_name, _items));
		}

		public static string ComputeHash(string _strToHash)
		{
			if (_strToHash != null && _strToHash.Length > 0)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(_strToHash);
				if (bytes != null)
				{
					try
					{
						SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
						return BitConverter.ToString(sHA1CryptoServiceProvider.ComputeHash(bytes)).Replace("-", "");
					}
					catch (Exception ex)
					{
						ExceptionMonitor.ExceptionRaised(ex, _strToHash, bytes);
						return string.Empty;
					}
				}
			}
			return string.Empty;
		}

		public static string ComputeInitialHash(long _v)
		{
			ulong num = (ulong)_v;
			char[] array = num.ToString().ToCharArray();
			Array.Reverse(array);
			string strToHash = "init:" + new string(array);
			return ComputeHash(strToHash);
		}

		public static string ComputeInitialHash(string _s)
		{
			char[] array = _s.ToCharArray();
			Array.Reverse(array);
			string strToHash = "init:" + new string(array);
			return ComputeHash(strToHash);
		}
	}
}
