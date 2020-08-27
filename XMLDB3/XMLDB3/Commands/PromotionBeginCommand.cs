using Mabinogi;
using System.Collections;

namespace XMLDB3
{
	public class PromotionBeginCommand : BasicCommand
	{
		private string m_serverName = string.Empty;

		private string m_channelName = string.Empty;

		private ArrayList m_skillId = new ArrayList();

		private bool m_Result;

		public PromotionBeginCommand()
			: base(NETWORKMSG.NET_DB_PROMOTION_BEGIN_TEST)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_serverName = _Msg.ReadString();
			m_channelName = _Msg.ReadString();
			while (true)
			{
				ushort num = _Msg.ReadU16();
				if (num == 0)
				{
					break;
				}
				m_skillId.Add(num);
			}
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PromotionBeginCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("PromotionBeginCommand.DoProcess() : 시험을 시작합니다.");
			foreach (ushort item in m_skillId)
			{
				m_Result = QueryManager.PromotionRank.BeginPromotion(m_serverName, m_channelName, item);
				if (!m_Result)
				{
					WorkSession.WriteStatus("PromotionBeginCommand.DoProcess() : 시험이 시작되지 못했습니다.");
					break;
				}
				WorkSession.WriteStatus("PromotionBeginCommand.DoProcess() :시험이 시작되었습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PromotionBeginCommand.MakeMessage() : 함수에 진입하였습니다");
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
