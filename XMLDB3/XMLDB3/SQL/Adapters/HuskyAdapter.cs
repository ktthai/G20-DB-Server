using System;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class HuskyAdapter : SqlAdapter
	{
		protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.Husky;

        public HuskyAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public bool Callprocedure(string _account, long _charId, string _charName)
		{
			WorkSession.WriteStatus("HuskySqlAdapter.Callprocedure() : 함수에 진입하였습니다.");
			// TODO: Judging from the File version of this class, it seems this will just be logged, and doesn't really require any call back

			return true;
			/*SqlConnection sqlConnection = new SqlConnection(strConnection);
			try
			{
				SqlCommand sqlCommand = new SqlCommand("__update_mabi_event_history", sqlConnection);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.Parameters.Add("@account", SqlDbType.VarChar, 64).Value = _account;
				sqlCommand.Parameters.Add("@characterid", SqlDbType.BigInt, 8).Value = _charId;
				sqlCommand.Parameters.Add("@charactername", SqlDbType.NVarChar, 64).Value = _charName;
				WorkSession.WriteStatus("HuskySqlAdapter.Callprocedure() : 데이터 베이스에 연결합니다.");
				sqlConnection.Open();
				sqlCommand.ExecuteScalar();
			}
			catch (SimpleSqlException ex)
			{
				ExceptionMonitor.ExceptionRaised(ex);
				WorkSession.WriteStatus(ex.Message, ex.Number);
			}
			catch (Exception ex2)
			{
				ExceptionMonitor.ExceptionRaised(ex2);
				WorkSession.WriteStatus(ex2.Message);
			}
			finally
			{
				WorkSession.WriteStatus("HuskySqlAdapter.Callprocedure() : 데이터 베이스에 연결을 종료합니다.");
				sqlConnection.Close();
			}
			return true;*/
		}
	}
}
