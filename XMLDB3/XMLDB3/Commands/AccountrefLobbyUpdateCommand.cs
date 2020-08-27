using Mabinogi;

namespace XMLDB3
{
	public class AccountrefLobbyUpdateCommand : BasicCommand
	{
		private string m_Account;

		private int m_LobbyOption;

		private bool m_bResult;

		private LobbyTabList m_CharLobbyTabList;

		private LobbyTabList m_PetLobbyTabList;

		public AccountrefLobbyUpdateCommand()
			: base(NETWORKMSG.MC_DB_ACCOUNT_REF_LOBBY_OPTION_UPDATE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Account = _Msg.ReadString();
			m_LobbyOption = _Msg.ReadS32();
			m_CharLobbyTabList = LobbyTabListSerializer.Serialize(_Msg);
			m_PetLobbyTabList = LobbyTabListSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("AccountrefLobbyUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("AccountrefLobbyUpdateCommand.DoProcess() : [" + m_Account + "] 가 로비설정을 기록합니다");
			m_bResult = QueryManager.Accountref.SetLobbyOption(m_Account, m_LobbyOption, m_CharLobbyTabList, m_PetLobbyTabList);
			if (m_bResult)
			{
				WorkSession.WriteStatus("AccountrefLobbyUpdateCommand.DoProcess() : [" + m_Account + "] 가 로비설정을 기록합니다");
				return true;
			}
			WorkSession.WriteStatus("AccountrefLobbyUpdateCommand.DoProcess() : [" + m_Account + "] 가 로비설정을 기록하지 못하였습니다");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("AccountrefLobbyUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_bResult)
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
