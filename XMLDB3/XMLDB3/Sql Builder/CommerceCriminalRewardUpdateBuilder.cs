using Mabinogi.SQL;

namespace XMLDB3
{
    public class CommerceCriminalRewardUpdateBuilder
    {
        public static void Build(int criminalId, int reward, SimpleConnection conn, SimpleTransaction transaction)
        {
            int rows = 0;
            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.CommerceCriminalReward, transaction))
            {
                cmd.Where(Mabinogi.SQL.Columns.CommerceCriminalReward.CriminalId, criminalId);
                cmd.Set(Mabinogi.SQL.Columns.CommerceCriminalReward.Reward, reward);
                rows = cmd.Execute();
            }

            if (rows == 0)
            {
                using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CommerceCriminalReward, transaction))
                {
                    cmd.Set(Mabinogi.SQL.Columns.CommerceCriminalReward.CriminalId, criminalId);
                    cmd.Set(Mabinogi.SQL.Columns.CommerceCriminalReward.Reward, reward);
                    cmd.Execute();
                }
            }
        }
    }
}
