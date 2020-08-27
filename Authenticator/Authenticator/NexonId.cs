using System;
using System.Data;
using System.Data.SqlClient;

namespace Authenticator
{
	public class NexonId
	{
		private string account;

		private string password;

		private bool isTestMode;

		public NexonId(string _account, string _password, bool _isTestMode)
		{
			account = _account;
			password = _password;
			isTestMode = _isTestMode;
		}

		public bool QueryNexonId(ref string nexonId)
		{
			//if (isTestMode)
			//{
				if (account.Length % 2 == 0)
				{
					nexonId = "qaguss20";
				}
				else
				{
					nexonId = "qaguss70";
				}
				return true;
			//}
			/*
			WorkSession.WriteStatus("NexonId.QueryNexonId(\"" + account + "\") : enter");
			SqlConnection sqlConnection = new SqlConnection(ServerConfiguration.WebDBConnectionString);
			SqlTransaction sqlTransaction = null;
			SqlCommand sqlCommand = new SqlCommand("usp_LoginCheck_NX", sqlConnection);
			sqlCommand.CommandType = CommandType.StoredProcedure;
			SqlParameter sqlParameter = sqlCommand.Parameters.Add("@MabiID", SqlDbType.NChar, 50);
			sqlParameter.Value = account;
			SqlParameter sqlParameter2 = sqlCommand.Parameters.Add("@MabiPWD", SqlDbType.NChar, 100);
			sqlParameter2.Value = password;
			SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
			sqlDataAdapter.TableMappings.Add("Table", "retVal");
			DataSet dataSet = new DataSet();
			try
			{
				WorkSession.WriteStatus("NexonId.QueryNexonId(\"" + account + "\") : open db connection");
				sqlConnection.Open();
				sqlTransaction = (sqlCommand.Transaction = sqlConnection.BeginTransaction());
				sqlDataAdapter.Fill(dataSet);
				sqlTransaction.Commit();
			}
			catch (Exception)
			{
				if (sqlTransaction != null)
				{
					WorkSession.WriteStatus("NexonId.QueryNexonId(\"" + account + "\") : rollback query");
					try
					{
						sqlTransaction.Rollback();
					}
					catch (Exception ex)
					{
						ExceptionMonitor.ExceptionRaised(ex);
					}
				}
				return false;
			}
			finally
			{
				WorkSession.WriteStatus("NexonId.QueryNexonId(\"" + account + "\") : close db connection");
				sqlConnection.Close();
			}
			DataTable dataTable = dataSet.Tables["retVal"];
			if (dataTable == null)
			{
				WorkSession.WriteStatus("NexonId.QueryNexonId(\"" + account + "\") : no table read, return null");
				return false;
			}
			DataRow dataRow = dataTable.Rows[0];
			string text = (string)dataRow[0];
			string[] array = text.Split('|');
			int num = 0;
			if (array.Length > 2)
			{
				try
				{
					num = int.Parse(array[0]);
				}
				catch (Exception)
				{
				}
				if (num == 1)
				{
					nexonId = array[2];
				}
				else
				{
					nexonId = "";
				}
			}
			else
			{
				nexonId = "";
			}
			WorkSession.WriteStatus("NexonId.QueryNexonId(\"" + account + "\") : result : " + nexonId);
			return true;
			*/
		}
	}
}
