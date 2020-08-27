using System.Text.Json;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class AchievementUpdateBuilder
    {
        public static void Build(Character _new, Character _old, SimpleConnection conn, SimpleTransaction transaction)
        {
            string text = JsonSerializer.Serialize(_new.achievements.achievement);
            string b = JsonSerializer.Serialize(_old.achievements.achievement);

            //PROCEDURE: UpdateAchievement
            if (text != b || _new.achievements.totalscore != _old.achievements.totalscore)
            {

                using (var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterAchievement, transaction))
                {
                    upCmd.Where(Mabinogi.SQL.Columns.CharacterAchievement.Id, _new.id);

                    upCmd.Set(Mabinogi.SQL.Columns.CharacterAchievement.TotalScore, _new.achievements.totalscore);
                    upCmd.Set(Mabinogi.SQL.Columns.CharacterAchievement.Achievement, text);

                    if (upCmd.Execute() < 1)
                    {
                        using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterAchievement, transaction))
                        {
                            insCmd.Set(Mabinogi.SQL.Columns.CharacterAchievement.Id, _new.id);

                            insCmd.Set(Mabinogi.SQL.Columns.CharacterAchievement.TotalScore, _new.achievements.totalscore);
                            insCmd.Set(Mabinogi.SQL.Columns.CharacterAchievement.Achievement, text);

                            insCmd.Execute();
                        }
                    }
                }
            }
        }
    }
}
