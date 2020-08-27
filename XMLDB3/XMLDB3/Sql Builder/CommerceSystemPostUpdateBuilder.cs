using Mabinogi.SQL;

namespace XMLDB3
{
    public class CommerceSystemPostUpdateBuilder
    {
        public static void Build(CommerceSystemPost _post, SimpleConnection conn, SimpleTransaction transaction)
        {
            int rows;
            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.CommercePost, transaction))
            {
                cmd.Where(Mabinogi.SQL.Columns.CommercePost.PostId, _post.id);
                cmd.Set(Mabinogi.SQL.Columns.CommercePost.PostCommission, _post.commission);
                cmd.Set(Mabinogi.SQL.Columns.CommercePost.PostInvestment, _post.investment);
                rows = cmd.Execute();
            }

            if (rows == 0)
            {
                using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CommercePost, transaction))
                {
                    cmd.Set(Mabinogi.SQL.Columns.CommercePost.PostId, _post.id);
                    cmd.Set(Mabinogi.SQL.Columns.CommercePost.PostCommission, _post.commission);
                    cmd.Set(Mabinogi.SQL.Columns.CommercePost.PostInvestment, _post.investment);
                    cmd.Execute();
                }
            }
        }
    }
}
