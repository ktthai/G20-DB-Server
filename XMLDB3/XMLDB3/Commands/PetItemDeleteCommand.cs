using Mabinogi;

namespace XMLDB3
{
	public class PetItemDeleteCommand : SerializedCommand
	{
		private long m_ID;

		private ItemList[] m_ItemList;

		private bool m_Result;

		public override bool ReplyEnable => false;

		public PetItemDeleteCommand()
			: base(NETWORKMSG.NET_DB_PET_ITEM_DELETE)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_ID = _Msg.ReadS64();
			m_ItemList = ItemListSerializer.Serialize(_Msg);
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.ObjectIDRegistant(m_ID);
			if (m_ItemList != null)
			{
				ItemList[] itemList = m_ItemList;
				foreach (ItemList itemList2 in itemList)
				{
					_helper.ObjectIDRegistant(itemList2.itemID);
				}
			}
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("PetItemDeleteCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("PetItemDeleteCommand.DoProcess() : [" + m_ID + "] 의 데이터를 캐쉬에서 읽습니다");
			PetInfo _cache = (PetInfo)ObjectCache.Character.Extract(m_ID);
			WorkSession.WriteStatus("PetItemDeleteCommand.DoProcess() : [" + m_ID + "] 의 아이템을 삭제합니다.");
			m_Result = QueryManager.Pet.DeleteItem(m_ID, m_ItemList, ref _cache);
			if (m_Result)
			{
				WorkSession.WriteStatus("PetItemDeleteCommand.DoProcess() : [" + m_ID + "] 의 아이템을 삭제하였습니다");
				if (_cache != null && _cache.inventory != null)
				{
					ObjectCache.Character.Push(m_ID, _cache);
				}
			}
			else
			{
				WorkSession.WriteStatus("PetItemDeleteCommand.DoProcess() : [" + m_ID + "] 의 아이템을 삭제에 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PetItemDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
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
