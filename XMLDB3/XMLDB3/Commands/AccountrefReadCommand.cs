using Mabinogi;
using System;

namespace XMLDB3
{
	public class AccountrefReadCommand : BasicCommand
	{
		private string m_strAccountref;

		private string m_strConnectedIp = string.Empty;

		private string m_strConnectedMachieId = string.Empty;

		private AccountRef m_ReadAccountref;

		private bool m_Result;

		public AccountrefReadCommand()
			: base(NETWORKMSG.MC_DB_ACCOUNT_REF_READ)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_strAccountref = _Msg.ReadString();
			m_strConnectedIp = _Msg.ReadString();
			m_strConnectedMachieId = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("AccountrefReadCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("AccountrefReadCommand.DoProcess() : [" + m_strAccountref + "] 게임계정을 읽습니다");
			m_ReadAccountref = QueryManager.Accountref.Read(m_strAccountref);
			if (m_ReadAccountref != null)
			{
				WorkSession.WriteStatus("AccountrefReadCommand.DoProcess() : [" + m_strAccountref + "] 게임계정을 읽었습니다");
				m_Result = true;
				if (m_strConnectedIp != string.Empty && m_strConnectedMachieId != string.Empty && (m_strConnectedIp != m_ReadAccountref.ip || m_strConnectedMachieId != m_ReadAccountref.mid))
				{
					if (m_strConnectedIp != m_ReadAccountref.ip && m_strConnectedMachieId != m_ReadAccountref.mid)
					{
						QueryManager.SetInfo.InsertSetInfo(m_strAccountref, m_strConnectedIp, m_strConnectedMachieId, DateTime.Now);
					}
					QueryManager.Accountref.UpdateAccountRefIPMID(m_strAccountref, m_strConnectedIp, m_strConnectedMachieId);
				}
				return true;
			}
			WorkSession.WriteStatus("AccountrefReadCommand.DoProcess() : [" + m_strAccountref + "] 게임계정을 읽지 못하였습니다");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("AccountrefReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result && m_ReadAccountref != null)
			{
				message.WriteU8(1);
				AccountrefSerializer.Deserialize(m_ReadAccountref, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
