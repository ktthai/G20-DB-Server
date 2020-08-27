using System.Text.Json;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class TitleObjectBuilder
	{
		//public class TitleContainer
		//{
		//	public CharacterTitles titles { get; set; }
		//}

		public static CharacterTitles Build(SimpleReader reader)
		{
			CharacterTitles titles= JsonSerializer.Deserialize<CharacterTitles>(reader.GetString(Mabinogi.SQL.Columns.Character.Title));
			return titles;
		}
	}
}
