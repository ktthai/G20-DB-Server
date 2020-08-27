using Mabinogi;

namespace XMLDB3
{
	public class CommerceSystemReadALL : BasicCommand
	{
		private string m_strServerName;

		private CommerceSystem m_commerceSystem;

		private REPLY_RESULT m_result;

		public CommerceSystemReadALL()
			: base(NETWORKMSG.NET_DB_COMMERCE_E_READ_ALL)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_strServerName = _message.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CommerceSystemReadALL.DoProcess() : 함수에 진입하였습니다");
			m_result = QueryManager.COCommerce.Read(m_strServerName, out m_commerceSystem);
			if (m_result != 0)
			{
				WorkSession.WriteStatus("CommerceSystemReadALL.DoProcess() : 교역물을 갱신하는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("CommerceSystemReadALL.DoProcess() : 교역물을 갱신하는데 실패했습니다.");
			}
			return m_result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CommerceSystemReadALL.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_result == REPLY_RESULT.SUCCESS && m_commerceSystem != null)
			{
				message.WriteU8(1);
				CommerceSystemPosts posts = m_commerceSystem.posts;
				if (posts == null || posts.postTable == null)
				{
					message.WriteS32(0);
					return message;
				}
				message.WriteS32(posts.postTable.Count);
				if (posts.postTable.Count == 0)
				{
					return message;
				}
				foreach (CommerceSystemPost value in posts.postTable.Values)
				{
					message.WriteS32(value.id);
					message.WriteS32(value.investment);
					message.WriteS32(value.commission);
				}
				CommerceSystemProducts products = m_commerceSystem.products;
				message.WriteS32(products.productTable.Count);
				{
					foreach (CommerceSystemProduct value2 in products.productTable.Values)
					{
						message.WriteS32(value2.id);
						message.WriteS32(value2.price);
						message.WriteS32(value2.count);
						message.WriteS32(value2.stockTable.Count);
						foreach (COStockInfo value3 in value2.stockTable.Values)
						{
							message.WriteS32(value3.idPost);
							message.WriteS32(value3.currentStock);
							message.WriteS32(value3.reserveStock);
							message.WriteS32(value3.price);
						}
					}
					return message;
				}
			}
			if (m_result == REPLY_RESULT.FAIL_EX)
			{
				message.WriteU8(51);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
