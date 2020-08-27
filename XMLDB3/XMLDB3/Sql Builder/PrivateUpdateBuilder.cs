using System.Text.Json;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class PrivateUpdateBuilder
	{
		private static string BuildReservedXmlData(CharacterPrivateReserved[] _reserveds)
		{
			if (_reserveds != null && _reserveds.Length > 0)
			{
				return JsonSerializer.Serialize(_reserveds);
			}
			return string.Empty;
		}

		private static string BuildBookXmlData(CharacterPrivateBook[] _books)
		{
			if (_books != null && _books.Length > 0)
			{
				return JsonSerializer.Serialize(_books);
			}
			return string.Empty;
		}

		public static bool Build(Character _new, Character _old, SimpleCommand cmd)
		{
			bool result = false;
			if (_new.Private != null && _old.Private != null)
			{
				string text = BuildReservedXmlData(_new.Private.reserveds);
				string b = BuildReservedXmlData(_old.Private.reserveds);

				string text2 = BuildBookXmlData(_new.Private.books);
				string b2 = BuildBookXmlData(_old.Private.books);

				if (text != b)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Reserved, text);
                    result = true;
                }
				if (text2 != b2)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Book, text2);
                    result = true;
                }
			}
			return result;
		}
	}
}
