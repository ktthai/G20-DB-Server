using System;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class PromotionAdapter : SqlAdapter
	{
		protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.PromotionRecord;

        public PromotionAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public bool BeginPromotion(string serverName, string channelName, ushort skillid)
		{
			WorkSession.WriteStatus("PromotionSqlAdapter.BeginPromotion() : empty function");
			return true;
        }

		public bool EndPromotion(string serverName, ushort skillid)
		{
			WorkSession.WriteStatus("PromotionSqlAdapter.EndPromotion() : empty function");

			return true;
        }

		public bool RecordScore(string serverName, string channelName, ushort skillid, string skillCategory, string skillName, ulong characterID, string characterName, byte race, ushort level, uint point)
		{
			WorkSession.WriteStatus("PromotionSqlAdapter.RecordScore() : empty function");
			return true;
        }
	}
}
