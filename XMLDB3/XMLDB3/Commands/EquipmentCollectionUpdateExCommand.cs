using Mabinogi;
using System;

namespace XMLDB3
{
	public class EquipmentCollectionUpdateExCommand : SerializedCommand
	{
		private CharacterInfo character_;

		private EquipmentCollection equipmentCollection_;

		private bool result_;

		private Message buildResultMsg_;

		private string desc;

		public EquipmentCollectionUpdateExCommand()
			: base(NETWORKMSG.NET_DB_EQUIPMENT_COLLECTION_SLOT_UPDATE_EX)
		{
		}

		protected override void _ReceiveData(Message _msg)
		{
			character_ = CharacterSerializer.Serialize(_msg);
			equipmentCollection_ = EquipmentCollectionSerializer.Serialize(_msg);
			desc = character_.id + "/" + character_.name;
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
			_helper.ObjectIDRegistant(character_.id);
			if (character_.inventory != null)
			{
				foreach (Item value in character_.inventory.Values)
				{
					_helper.ObjectIDRegistant(value.id);
				}
			}
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("EquipmentCollectionUpdateExCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("EquipmentCollectionUpdateExCommand.DoProcess() : [" + equipmentCollection_.Account + "] 의 데이터를 캐쉬에서 읽습니다");
			EquipmentCollectionCache equipmentCollectionCache = (EquipmentCollectionCache)ObjectCache.EquipmentCollection.Extract(equipmentCollection_.Account);
			if (equipmentCollectionCache == null)
			{
				equipmentCollectionCache = new EquipmentCollectionCache();
			}
			WorkSession.WriteStatus("EquipmentCollectionUpdateExCommand.DoProcess() : [" + desc + "] 의 데이터를 캐쉬에서 읽습니다");
			CharacterInfo charCache = (CharacterInfo)ObjectCache.Character.Extract(character_.id);
			WorkSession.WriteStatus("EquipmentCollectionUpdateExCommand.DoProcess() : [" + equipmentCollection_.Account + "] 의 데이터를 저장합니다");
			result_ = QueryManager.EquipmentCollection.WriteEx(equipmentCollection_, character_, equipmentCollectionCache, charCache, QueryManager.Character, out buildResultMsg_);
			if (!result_)
			{
				WorkSession.WriteStatus("EquipmentCollectionUpdateExCommand.DoProcess() : [" + equipmentCollection_.Account + "] 의 데이터 저장에 실패하였습니다. 다시 시도합니다");
				equipmentCollectionCache = new EquipmentCollectionCache();
				result_ = QueryManager.EquipmentCollection.WriteEx(equipmentCollection_, character_, equipmentCollectionCache, null, QueryManager.Character, out buildResultMsg_);
			}
			if (result_)
			{
				WorkSession.WriteStatus("EquipmentCollectionUpdateExCommand.DoProcess() : [" + equipmentCollection_.Account + "] 의 데이터를 업데이트 하였습니다");
				ObjectCache.EquipmentCollection.Push(equipmentCollection_.Account, equipmentCollectionCache);
				ObjectCache.Character.Push(character_.id, character_);
			}
			else
			{
				WorkSession.WriteStatus("EquipmentCollectionUpdateExCommand.DoProcess() : [" + equipmentCollection_.Account + "] 의 데이터 저장에 실패하였습니다");
				ExceptionMonitor.ExceptionRaised(new Exception("[" + equipmentCollection_.Account + "] 의 데이터 저장에 실패하였습니다"));
			}
			return result_;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("EquipmentCollectionUpdateExCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8(1);
			return message + buildResultMsg_;
		}
	}
}
