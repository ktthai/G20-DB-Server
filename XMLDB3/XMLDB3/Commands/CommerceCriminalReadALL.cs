using Mabinogi;

namespace XMLDB3
{
	public class CommerceCriminalReadALL : BasicCommand
	{
		private string m_strServerName;

		private CommerceCriminals m_commerceCriminals;

		private REPLY_RESULT m_result;

		public CommerceCriminalReadALL()
			: base(NETWORKMSG.NET_DB_COMMERCE_CRIMINAL_READ_ALL)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_strServerName = _message.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CommerceCriminalReadALL.DoProcess() : 함수에 진입하였습니다");
			m_result = QueryManager.CCCommerce.ReadAll(m_strServerName, out m_commerceCriminals);
			if (m_result != 0)
			{
				WorkSession.WriteStatus("CommerceCriminalReadALL.DoProcess() : 현상범 정보를 읽는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("CommerceCriminalReadALL.DoProcess() : 현상범 정보를 읽는데 실패했습니다.");
			}
			return m_result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CommerceCriminalReadALL.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_result == REPLY_RESULT.SUCCESS && m_commerceCriminals != null)
			{
				if (m_commerceCriminals.criminalTable == null)
				{
					message.WriteU8(0);
					return message;
				}
				message.WriteU8(1);
				message.WriteS32(m_commerceCriminals.criminalTable.Count);
				if (m_commerceCriminals.criminalTable.Count == 0)
				{
					return message;
				}
				{
					foreach (CommerceCriminal value in m_commerceCriminals.criminalTable.Values)
					{
						message.WriteS32(value.id);
						message.WriteS32(value.reward);
						if (value.info == null)
						{
							message.WriteS32(0);
						}
						else if (value.info.criminalTable == null)
						{
							message.WriteS32(0);
						}
						else
						{
							message.WriteS32(value.info.criminalTable.Count);
							foreach (CCommerceCriminalLost value2 in value.info.criminalTable.Values)
							{
								message.WriteString(value2.charName);
								message.WriteS32(value2.stolenDucat);
							}
						}
					}
					return message;
				}
			}
			message.WriteU8(0);
			return message;
		}
	}
}
