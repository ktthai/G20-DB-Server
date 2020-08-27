using Mabinogi;

namespace XMLDB3
{
	public class EquipmentCollectionRankCountListReadCommand : SerializedCommand
	{
		private CollectionCountList collectionCountList_;

		private bool isSuccess;

		public EquipmentCollectionRankCountListReadCommand()
			: base(NETWORKMSG.NET_DB_EQUIPMENT_COLLECTION_RANK_READ)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("EquipmentCollectionRankCountListReadCommand.DoProcess() : 함수에 진입하였습니다");
			collectionCountList_ = QueryManager.EquipmentCollection.ReadCollectionCountList();
			isSuccess = (null != collectionCountList_);
			return isSuccess;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("EquipmentCollectionRankCountListReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (isSuccess)
			{
				message.WriteU8(1);
				EquipmentCollectionCountListSerializer.Deserialize(collectionCountList_, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
