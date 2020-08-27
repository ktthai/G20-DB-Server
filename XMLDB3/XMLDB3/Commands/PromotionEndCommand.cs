using Mabinogi;
using System.Collections;

namespace XMLDB3
{
	public class PromotionEndCommand : BasicCommand
	{
		private string m_serverName = string.Empty;

		private ArrayList m_skillId = new ArrayList();

		private bool m_Result;

		public PromotionEndCommand()
			: base(NETWORKMSG.NET_DB_PROMOTION_END_TEST)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_serverName = _Msg.ReadString();
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
			WorkSession.WriteStatus("PromotionEndCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("PromotionEndCommand.DoProcess() : 시험을 시작합니다.");
			foreach (ushort item in m_skillId)
			{
				m_Result = QueryManager.PromotionRank.EndPromotion(m_serverName, item);
				if (!m_Result)
				{
					WorkSession.WriteStatus("PromotionEndCommand.DoProcess() : 시험이 시작되지 못했습니다.");
					break;
				}
				WorkSession.WriteStatus("PromotionEndCommand.DoProcess() : 시험이 시작되었습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PromotionEndCommand.MakeMessage() : 함수에 진입하였습니다");
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
