using System;
using System.Collections;
using System.Text;

namespace XMLDB3
{
	public class CMetaHelper
	{
		public static string MeteRowListToMetaString(Hashtable list)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (character_meta_row value in list.Values)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(";");
				}
				stringBuilder.Append(value.ToMetaString());
			}
			return stringBuilder.ToString();
		}

		public static Hashtable MetaStringToMetaRowList(long charID, string MetaString)
		{
			if (MetaString == null || MetaString.Length == 0)
			{
				throw new Exception("Meta string is empty.");
			}
			if (MetaString[MetaString.Length - 1] == ';')
			{
				MetaString = MetaString.Remove(MetaString.Length - 1, 1);
			}
			Hashtable hashtable = new Hashtable();
			string[] array = MetaString.Split(";".ToCharArray());
			string[] array2 = array;
			foreach (string text in array2)
			{
				string[] array3 = text.Split(":".ToCharArray());
				character_meta_row character_meta_row = new character_meta_row();
				character_meta_row.charID = charID;
				character_meta_row.mcode = array3[0];
				character_meta_row.mtype = array3[1];
				character_meta_row.mdata = array3[2];
				hashtable.Add(character_meta_row.mcode, character_meta_row);
			}
			return hashtable;
		}
	}
}
