using Mabinogi.SQL;

namespace XMLDB3
{
    public class CommerceCriminalUpdateBuilder
    {
        public static void Build(int _criminalId, string _charName, int _ducat, SimpleConnection conn, SimpleTransaction transaction)
        {
            int rows = 0;
            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.CommerceCriminal, transaction))
            {
                cmd.Where(Mabinogi.SQL.Columns.CommerceCriminal.CriminalId, _criminalId);
                cmd.Where(Mabinogi.SQL.Columns.CommerceCriminal.CharName, _charName);
                cmd.Set(Mabinogi.SQL.Columns.CommerceCriminal.Ducat, _ducat);
                rows = cmd.Execute();
            }

            if (rows == 0)
            {
                using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CommerceCriminal, transaction))
                {
                    cmd.Set(Mabinogi.SQL.Columns.CommerceCriminal.CriminalId, _criminalId);
                    cmd.Set(Mabinogi.SQL.Columns.CommerceCriminal.CharName, _charName);
                    cmd.Set(Mabinogi.SQL.Columns.CommerceCriminal.Ducat, _ducat);
                    cmd.Execute();
                }
            }
        }
    }
}
