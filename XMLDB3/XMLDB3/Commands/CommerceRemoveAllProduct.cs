using Mabinogi;

namespace XMLDB3
{
	public class CommerceRemoveAllProduct : BasicCommand
	{
		private long m_charId;

		private long m_ducat;

		private REPLY_RESULT m_result;

		public CommerceRemoveAllProduct()
			: base(NETWORKMSG.NET_DB_COMMERCE_T_REMOVE_ALL_PRODUCT)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_charId = _message.ReadS64();
			m_ducat = _message.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CommerceRemoveAllProduct.DoProcess() : 함수에 진입하였습니다");
			m_result = QueryManager.GSCommerce.RemoveAllProduct(m_charId, m_ducat);
			if (m_result != 0)
			{
				WorkSession.WriteStatus("CommerceRemoveAllProduct.DoProcess() : 교역물 정보를 모두 삭제하는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("CommerceRemoveAllProduct.DoProcess() : 교역물 정보를 모두 삭제하는데 실패했습니다.");
			}
			return m_result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CommerceRemoveAllProduct.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_result);
			return message;
		}
	}
}
