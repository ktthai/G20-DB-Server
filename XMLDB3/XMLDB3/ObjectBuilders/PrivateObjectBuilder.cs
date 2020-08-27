using Mabinogi.SQL;
using System.Collections.Generic;
using System.Text.Json;

namespace XMLDB3
{
	public class PrivateObjectBuilder
	{
		
		public static CharacterPrivate BuildCharacterPrivate(SimpleReader reader)
		{
            CharacterPrivate characterPrivate = new CharacterPrivate();
            characterPrivate.reserveds = JsonSerializer.Deserialize<CharacterPrivateReserved[]>(reader.GetString(Mabinogi.SQL.Columns.Character.Reserved));
            characterPrivate.books = JsonSerializer.Deserialize<CharacterPrivateBook[]>(reader.GetString(Mabinogi.SQL.Columns.Character.Book));
			return characterPrivate;
        }

		public static CharacterPrivateRegistered[] BuildPrivateRegistered(SimpleReader reader)
		{
			List<CharacterPrivateRegistered> privateRegistereds = new List<CharacterPrivateRegistered>();
			CharacterPrivateRegistered characterPrivateRegistered;
			while (reader.Read())
			{
				characterPrivateRegistered = new CharacterPrivateRegistered();
				characterPrivateRegistered.id = reader.GetInt32(Mabinogi.SQL.Columns.CharacterQuest.QuestId);
				characterPrivateRegistered.start = reader.GetInt64(Mabinogi.SQL.Columns.CharacterQuest.Start);
				characterPrivateRegistered.end = reader.GetInt64(Mabinogi.SQL.Columns.CharacterQuest.End);
				characterPrivateRegistered.extra = reader.GetInt32(Mabinogi.SQL.Columns.CharacterQuest.Extra);
				privateRegistereds.Add(characterPrivateRegistered);
			}
			return privateRegistereds.ToArray();
		}
	}
}
