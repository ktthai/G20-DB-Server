using Mabinogi;

namespace XMLDB3
{
	public class PetReadCommand : SerializedCommand
	{
		private PetInfo m_ReadPet;

		private string m_Account = string.Empty;

		private string m_Server = string.Empty;

		private long m_Id;

		public PetReadCommand()
			: base(NETWORKMSG.NET_DB_PET_READ)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_Account = _Msg.ReadString();
			m_Server = _Msg.ReadString();
			m_Id = (long)_Msg.ReadU64();
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.ObjectIDRegistant(m_Id);
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("PetReadCommand.DoProcess() : [" + m_Id + "] 를  캐쉬에서 읽기를 시도합니다");
			PetInfo cache = (PetInfo)ObjectCache.Character.Extract(m_Id);
			m_ReadPet = QueryManager.Pet.Read(m_Account, m_Server, m_Id, cache, QueryManager.Accountref);
			if (m_ReadPet != null)
			{
				WorkSession.WriteStatus("PetReadCommand.DoProcess() : [" + m_Id + "] 를 데이터베이스에서 읽었습니다");
				ObjectCache.Character.Push(m_Id, m_ReadPet);
				if (!InventoryHashUtility.CheckHash(m_ReadPet.inventoryHash, m_ReadPet.strToHash, m_ReadPet.id, m_ReadPet.updatetime))
				{
					MailSender.Send("hashMissmatch - PetRead", "PetId: " + m_ReadPet.id + "(" + m_ReadPet.name + ")" + "\tHash: " + m_ReadPet.inventoryHash + " " + m_ReadPet.strToHash);
					if (ConfigManager.DoesCheckHash)
					{
						m_ReadPet = null;
					}
				}
			}
			else
			{
				WorkSession.WriteStatus("PetReadCommand.DoProcess() : [" + m_Id + "] 를 데이터베이스에 쿼리하는데 실패하였습니다");
			}
			return m_ReadPet != null;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PetReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_ReadPet != null)
			{
				message.WriteU8(1);
				PetSerializer.Deserialize(m_ReadPet, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
