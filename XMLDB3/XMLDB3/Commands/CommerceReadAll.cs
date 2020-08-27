using Mabinogi;

namespace XMLDB3
{
	public class CommerceReadAll : BasicCommand
	{
		private long m_charId;

		private GS_Commerce m_commerce;

		private REPLY_RESULT m_result;

		public CommerceReadAll()
			: base(NETWORKMSG.NET_DB_COMMERCE_T_READ_ALL_DATA)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_commerce = new GS_Commerce();
			m_charId = _message.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CommerceReadAll.DoProcess() : 함수에 진입하였습니다");
			m_result = QueryManager.GSCommerce.ReadAll(m_charId, out m_commerce);
			if (m_result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("CommerceReadAll.DoProcess() : 무역 데이타 모든 데이터를 읽는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("CommerceReadAll.DoProcess() : 무역 데이타 모든 데이터를 읽는데 실패했습니다.");
			}
			return m_result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CommerceReadAll.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_result == REPLY_RESULT.SUCCESS && m_commerce != null)
			{
				message.WriteU8(1);
				message.WriteS64(m_commerce.info.ducat);
				message.WriteS64(m_commerce.info.unlockTransport);
				message.WriteS32(m_commerce.info.currentTransport);
				message.WriteS32(m_commerce.info.lost_percent);
				if (m_commerce.info.credibilityTable == null)
				{
					message.WriteS32(0);
				}
				else
				{
					message.WriteS32(m_commerce.info.credibilityTable.Count);
					foreach (CommerceCredibility value in m_commerce.info.credibilityTable.Values)
					{
						message.WriteS32(value.postId);
						message.WriteS32(value.credibility);
					}
				}
				if (m_commerce.products.productTable != null)
				{
					message.WriteS32(m_commerce.products.productTable.Count);
					{
						foreach (CommercePurchasedProduct value2 in m_commerce.products.productTable.Values)
						{
							message.WriteS32(value2.id);
							message.WriteS32(value2.count);
							message.WriteS32(value2.price);
						}
						return message;
					}
				}
				message.WriteS32(0);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
