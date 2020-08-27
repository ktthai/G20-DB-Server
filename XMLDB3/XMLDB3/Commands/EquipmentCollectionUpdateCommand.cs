using Mabinogi;
using System;

namespace XMLDB3
{
	public class EquipmentCollectionUpdateCommand : SerializedCommand
	{
		private EquipmentCollection equipmentCollection_;

		private bool result_;

		public EquipmentCollectionUpdateCommand()
			: base(NETWORKMSG.NET_DB_EQUIPMENT_COLLECTION_SLOT_UPDATE)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			equipmentCollection_ = EquipmentCollectionSerializer.Serialize(_Msg);
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.StringIDRegistant(equipmentCollection_.Account);
			if (equipmentCollection_ != null && equipmentCollection_.item != null)
			{
				foreach (CollectionItem collectionItem in equipmentCollection_.item)
				{
					_helper.ObjectIDRegistant(collectionItem.item.id);
				}
			}
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("EquipmentCollectionUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("EquipmentCollectionUpdateCommand.DoProcess() : [" + equipmentCollection_.Account + "] 의 데이터를 캐쉬에서 읽습니다");
			EquipmentCollectionCache equipmentCollectionCache = (EquipmentCollectionCache)ObjectCache.EquipmentCollection.Extract(equipmentCollection_.Account);
			if (equipmentCollectionCache == null)
			{
				equipmentCollectionCache = new EquipmentCollectionCache();
			}
			WorkSession.WriteStatus("EquipmentCollectionUpdateCommand.DoProcess() : [" + equipmentCollection_.Account + "] 의 데이터를 저장합니다");
			result_ = QueryManager.EquipmentCollection.Write(equipmentCollection_, equipmentCollectionCache);
			if (!result_)
			{
				WorkSession.WriteStatus("EquipmentCollectionUpdateCommand.DoProcess() : [" + equipmentCollection_.Account + "] 의 데이터 저장에 실패하였습니다. 다시 시도합니다");
				equipmentCollectionCache = new EquipmentCollectionCache();
				result_ = QueryManager.EquipmentCollection.Write(equipmentCollection_, equipmentCollectionCache);
			}
			if (result_)
			{
				WorkSession.WriteStatus("EquipmentCollectionUpdateCommand.DoProcess() : [" + equipmentCollection_.Account + "] 의 데이터를 업데이트 하였습니다");
				ObjectCache.EquipmentCollection.Push(equipmentCollection_.Account, equipmentCollectionCache);
			}
			else
			{
				WorkSession.WriteStatus("EquipmentCollectionUpdateCommand.DoProcess() : [" + equipmentCollection_.Account + "] 의 데이터 저장에 실패하였습니다");
				ExceptionMonitor.ExceptionRaised(new Exception("[" + equipmentCollection_.Account + "] 의 데이터 저장에 실패하였습니다"));
			}
			return result_;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("EquipmentCollectionUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (result_)
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
