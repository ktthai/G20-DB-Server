using Mabinogi;
using System.Collections;

namespace XMLDB3
{
	public class PromotionRecordScoreCommand : BasicCommand
	{
		private string m_serverName;

		private string m_channelName;

		private ushort m_skillid;

		private string m_skillCategory;

		private string m_skillName;

		private bool m_Result;

		private ArrayList m_characterID = new ArrayList();

		private ArrayList m_characterName = new ArrayList();

		private ArrayList m_race = new ArrayList();

		private ArrayList m_level = new ArrayList();

		private ArrayList m_point = new ArrayList();

		public PromotionRecordScoreCommand()
			: base(NETWORKMSG.NET_DB_PROMOTION_RECORD_POINT)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_serverName = _Msg.ReadString();
			m_channelName = _Msg.ReadString();
			m_skillCategory = _Msg.ReadString();
			m_skillid = _Msg.ReadU16();
			m_skillName = _Msg.ReadString();
			while (true)
			{
				byte b = _Msg.ReadU8();
				if (b == byte.MaxValue)
				{
					break;
				}
				m_race.Add(b);
				m_characterID.Add(_Msg.ReadU64());
				m_characterName.Add(_Msg.ReadString());
				m_level.Add(_Msg.ReadU16());
				m_point.Add(_Msg.ReadU32());
			}
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PromotionRecordScoreCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("PromotionRecordScoreCommand.DoProcess() : 점수를 기록합니다.");
			for (int i = 0; i < m_characterID.Count; i++)
			{
				m_Result = QueryManager.PromotionRank.RecordScore(m_serverName, m_channelName, m_skillid, m_skillCategory, m_skillName, (ulong)m_characterID[i], (string)m_characterName[i], (byte)m_race[i], (ushort)m_level[i], (uint)m_point[i]);
				if (m_Result)
				{
					WorkSession.WriteStatus("PromotionRecordScoreCommand.DoProcess() :점수를 기록하였습니다..");
					continue;
				}
				WorkSession.WriteStatus("PromotionRecordScoreCommand.DoProcess() : 점수를 기록하지 못했습니다.");
				break;
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PromotionRecordScoreCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
