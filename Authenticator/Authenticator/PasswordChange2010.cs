using System;

namespace Authenticator
{
	public class PasswordChange2010
	{
		private string account;

		private bool isTestMode;

		public PasswordChange2010(string _account, bool _isTestMode)
		{
			account = _account;
			isTestMode = _isTestMode;
		}

		public bool QueryShowInformation()
		{
			//if (isTestMode)
			{
				return false;
			}
			/*
			WorkSession.WriteStatus("PasswordChange2010.Load(\"" + account + "\") : enter");
			SqlConnection sqlConnection = new SqlConnection(ServerConfiguration.Passwordchange2010ConnectionString);
			SqlCommand sqlCommand = new SqlCommand("usp_ShowSecurityCampaign", sqlConnection);
			sqlCommand.CommandType = CommandType.StoredProcedure;
			SqlParameter sqlParameter = sqlCommand.Parameters.Add("@UserID", SqlDbType.Char, 32);
			sqlParameter.Value = account;
			sqlParameter = sqlCommand.Parameters.Add("@Show", SqlDbType.Int);
			sqlParameter.Direction = ParameterDirection.Output;
			try
			{
				WorkSession.WriteStatus("PasswordChange2010.Load(\"" + account + "\") : open db connection");
				sqlConnection.Open();
				sqlCommand.ExecuteNonQuery();
				sqlParameter = sqlCommand.Parameters["@Show"];
				if (sqlParameter != null)
				{
					return (int)sqlParameter.Value == 1;
				}
				return false;
			}
			catch (Exception ex)
			{
				WorkSession.WriteStatus("PasswordChange2010.Load(\"" + account + "\") : Error: " + ex.ToString());
				return false;
			}
			finally
			{
				WorkSession.WriteStatus("PasswordChange2010.Load(\"" + account + "\") : close db connection");
				sqlConnection.Close();
			}
			*/
		}
	}
}
