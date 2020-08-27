using System;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class SetInfoAdapter : SqlAdapter
	{
		protected override string ConfigRef => "";

        public SetInfoAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        public long InsertSetInfo(string _account, string _connectedIp, string _connectedMachineId, DateTime _snapshotDate)
		{
			WorkSession.WriteStatus("SetInfoSqlAdapter.InsertSetInfo() : 함수에 진입하였습니다");
			return 0;
		}
	}
}
