using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace XMLDB3
{
	public class CAccountMetaHelper
	{
		public static string AccountMetaRowListToMetaString(Hashtable _list)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (AccountrefMeta value in _list.Values)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(";");
				}
				stringBuilder.Append(AccountMetaToString(value));
			}
			return stringBuilder.ToString();
		}

		public static Hashtable AccountMetaStringToMetaRowList(string _metaString)
		{
			if (_metaString == null || _metaString.Length == 0)
			{
				throw new Exception("Account Meta string is empty.");
			}
			if (_metaString[_metaString.Length - 1] == ';')
			{
				_metaString = _metaString.Remove(_metaString.Length - 1, 1);
			}
			Hashtable hashtable = new Hashtable();
			string[] array = _metaString.Split(";".ToCharArray());
			string[] array2 = array;
			foreach (string text in array2)
			{
				string[] array3 = text.Split(":".ToCharArray());
				AccountrefMeta accountrefMeta = new AccountrefMeta();
				accountrefMeta.mcode = array3[0];
				accountrefMeta.mtype = array3[1];
				accountrefMeta.mdata = array3[2];
				hashtable.Add(accountrefMeta.mcode, accountrefMeta);
			}
			return hashtable;
		}

		public static string AccountMetaToString(AccountrefMeta _meta)
		{
			if (_meta != null)
			{
				return _meta.mcode + ":" + _meta.mtype + ":" + _meta.mdata;
			}
			return string.Empty;
		}

		public static string AccountMetaListToString(List<AccountrefMeta> _metaList)
		{
			if (_metaList != null && _metaList.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (AccountrefMeta meta in _metaList)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(";");
					}
					stringBuilder.Append(AccountMetaToString(meta));
				}
				return stringBuilder.ToString();
			}
			return string.Empty;
		}
	}
}
