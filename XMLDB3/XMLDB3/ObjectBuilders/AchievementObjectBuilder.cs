using System;
using System.Text.Json;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class AchievementObjectBuilder
	{
		public static CharacterAchievements Build(SimpleReader reader)
		{
			CharacterAchievements characterAchievements = new CharacterAchievements();
			if (reader.Read())
			{
				characterAchievements.totalscore = reader.GetInt32(Mabinogi.SQL.Columns.CharacterAchievement.TotalScore);
				characterAchievements.achievement = JsonSerializer.Deserialize<CharacterAchievementsAchievement[]>(reader.GetString(Mabinogi.SQL.Columns.CharacterAchievement.Achievement));
			}
			return characterAchievements;
		}
	}
}
