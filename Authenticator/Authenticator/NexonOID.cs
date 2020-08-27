using System;

namespace Authenticator
{
	public class NexonOID
	{
		private string mabinogiID;

		private bool isTestMode;

		public NexonOID(string _mabinogiID, bool _isTestMode)
		{
			mabinogiID = _mabinogiID;
			isTestMode = _isTestMode;
		}

		public bool QueryNexonOID(ref long _nexonOID)
		{
			//if (isTestMode)
			{
				if (mabinogiID.Contains("_otp"))
				{
					if (mabinogiID.Contains("_otpnet"))
					{
						_nexonOID = 1100L;
					}
					else
					{
						_nexonOID = 1000L;
					}
				}
				else if (mabinogiID.Contains("_oid"))
				{
					_nexonOID = 1L;
				}
				else
				{
					_nexonOID = 0L;
				}
				return true;
			}
			/*
			WorkSession.WriteStatus("NexonOID.QueryNexonOID(\"" + mabinogiID + "\") : enter");
			SqlConnection sqlConnection = new SqlConnection(ServerConfiguration.WebDBConnectionString);
			SqlCommand sqlCommand = new SqlCommand("usp_GetNexonOidByMabiID", sqlConnection);
			sqlCommand.CommandType = CommandType.StoredProcedure;
			SqlParameter sqlParameter = sqlCommand.Parameters.Add("@mabiID", SqlDbType.VarChar, 32);
			sqlParameter.Value = mabinogiID;
			sqlParameter.Direction = ParameterDirection.Input;
			SqlParameter sqlParameter2 = sqlCommand.Parameters.Add("@nxOID", SqlDbType.BigInt);
			sqlParameter2.Direction = ParameterDirection.Output;
			try
			{
				WorkSession.WriteStatus("NexonOID.QueryNexonOID(\"" + mabinogiID + "\") : open db connection");
				sqlConnection.Open();
				sqlCommand.ExecuteNonQuery();
				SqlParameter sqlParameter3 = sqlCommand.Parameters["@nxOID"];
				if (sqlParameter3 != null)
				{
					_nexonOID = (long)sqlParameter3.Value;
					WorkSession.WriteStatus("NexonOID.QueryNexonOID(\"" + mabinogiID + "\") : result : " + _nexonOID);
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				WorkSession.WriteStatus("NexonOID.QueryNexonOID(\"" + mabinogiID + "\") : Error: " + ex.ToString());
				return false;
			}
			finally
			{
				WorkSession.WriteStatus("NexonOID.QueryNexonOID(\"" + mabinogiID + "\") : close db connection");
				sqlConnection.Close();
			}
			*/
		}
	}
}
