using Mabinogi.SQL;
using System.Text.Json;

namespace XMLDB3
{
	public class TitleUpdateBuilder
	{
		private static string BuildTitleXmlData(CharacterTitles _title)
		{
			if (_title.title != null)
			{
				return JsonSerializer.Serialize(_title);
			}
			return JsonSerializer.Serialize(new CharacterTitles());
		}

		public static bool Build(Character _new, Character _old, SimpleCommand cmd)
		{
			bool result = false;
			if (_new.titles != null && _old.titles != null)
			{
				string text = BuildTitleXmlData(_new.titles);
				string b = BuildTitleXmlData(_old.titles);
				if (text != b)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Title, text);
                    result = true;
                }
			}
			return result;
		}
	}
}
