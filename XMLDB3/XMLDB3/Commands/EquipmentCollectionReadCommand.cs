using Mabinogi;

namespace XMLDB3
{
	public class EquipmentCollectionReadCommand : SerializedCommand
	{
		private EquipmentCollection equipmentCollection_;

		private string accountId_ = string.Empty;

		public EquipmentCollectionReadCommand()
			: base(NETWORKMSG.NET_DB_EQUIPMENT_COLLECTION_SLOT_READ)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			accountId_ = _Msg.ReadString();
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.StringIDRegistant(accountId_);
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("EquipmentCollectionReadCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("EquipmentCollectionReadCommand.DoProcess() : [" + accountId_ + "] 를 캐쉬에서 읽도록 시도합니다.");
			EquipmentCollectionCache equipmentCollectionCache = (EquipmentCollectionCache)ObjectCache.EquipmentCollection.Extract(accountId_);
			equipmentCollection_ = QueryManager.EquipmentCollection.Read(accountId_, equipmentCollectionCache);
			if (equipmentCollection_ != null && equipmentCollection_.IsValid())
			{
				WorkSession.WriteStatus("EquipmentCollectionReadCommand.DoProcess() : [" + accountId_ + "] 의 정보를 데이터베이스에서 읽었습니다");
				ObjectCache.EquipmentCollection.Push(accountId_, equipmentCollectionCache);
			}
			else
			{
				WorkSession.WriteStatus("EquipmentCollectionReadCommand.DoProcess() : [" + accountId_ + "] 의 정보를 데이터베이스에 쿼리하는데 실패하였습니다");
			}
			return equipmentCollection_ != null;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("EquipmentCollectionReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (equipmentCollection_ != null)
			{
				message.WriteU8(1);
				EquipmentCollectionSerializer.Deserialize(equipmentCollection_, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
