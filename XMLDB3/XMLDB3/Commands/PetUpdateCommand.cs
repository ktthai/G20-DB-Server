using Mabinogi;
using System;

namespace XMLDB3
{
	public class PetUpdateCommand : SerializedCommand
	{
		private PetInfo m_WritePet;

		private long m_Id;

		private string m_Name = string.Empty;

		private string m_Account = string.Empty;

		private string m_Server = string.Empty;

		private byte m_ChannelGroupId;

		private bool m_Result;

		public PetUpdateCommand()
			: base(NETWORKMSG.NET_DB_PET_UPDATE)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_Account = _Msg.ReadString();
			m_Server = _Msg.ReadString();
			m_ChannelGroupId = _Msg.ReadU8();
			m_WritePet = PetSerializer.Serialize(_Msg);
			m_Id = m_WritePet.id;
			m_Name = m_WritePet.name;
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.ObjectIDRegistant(m_Id);
			if (m_WritePet.inventory != null)
			{
				foreach (Item value in m_WritePet.inventory.Values)
				{
					_helper.ObjectIDRegistant(value.id);
				}
			}
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("PetUpdateCommand.DoProcess() : [" + m_Id + "/" + m_Name + "] 의 데이터를 캐쉬에서 읽습니다");
			PetInfo cache = (PetInfo)ObjectCache.Character.Extract(m_Id);
			WorkSession.WriteStatus("PetUpdateCommand.DoProcess() : [" + m_Id + "/" + m_Name + "] 의 데이터를 업데이트합니다");
			m_Result = QueryManager.Pet.Write(m_Account, m_Server, m_ChannelGroupId, m_WritePet, cache, QueryManager.Accountref);
			if (!m_Result)
			{
				WorkSession.WriteStatus("PetUpdateCommand.DoProcess() : [" + m_Id + "/" + m_Name + "] 의 데이터 저장에 실패하였습니다. 다시 시도합니다");
				m_Result = QueryManager.Pet.Write(m_Account, m_Server, m_ChannelGroupId, m_WritePet, null, QueryManager.Accountref);
			}
			if (m_Result)
			{
				WorkSession.WriteStatus("PetUpdateCommand.DoProcess() : [" + m_Id + "/" + m_Name + "] 의 데이터를 업데이트 하였습니다");
				ObjectCache.Character.Push(m_Id, m_WritePet);
			}
			else
			{
				WorkSession.WriteStatus("PetUpdateCommand.DoProcess() : [" + m_Id + "/" + m_Name + "] 의 데이터 저장에 실패하였습니다");
				ExceptionMonitor.ExceptionRaised(new Exception("[" + m_Id + "/" + m_Name + "] 의 데이터 저장에 실패하였습니다"));
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PetUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
				message.WriteU64((ulong)m_Id);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
