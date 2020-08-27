using Mabinogi;
using System.Collections;
using System.Collections.Generic;

namespace XMLDB3
{
	public class CommerceUpdateTransport : BasicCommand
	{
		private long m_charId;

		private int m_lostPercent;

		private CommercePurchasedProducts m_commerceProducts;

		private REPLY_RESULT m_result;

		public CommerceUpdateTransport()
			: base(NETWORKMSG.NET_DB_COMMERCE_T_END_COMBAT)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_commerceProducts = new CommercePurchasedProducts();
			m_commerceProducts.productTable = new Dictionary<int, CommercePurchasedProduct>();
			m_charId = _message.ReadS64();
			m_lostPercent = _message.ReadS32();
			int num = _message.ReadS32();
			for (int i = 0; i < num; i++)
			{
				CommercePurchasedProduct commercePurchasedProduct = new CommercePurchasedProduct();
				commercePurchasedProduct.id = _message.ReadS32();
				commercePurchasedProduct.count = _message.ReadS32();
				commercePurchasedProduct.price = _message.ReadS32();
				if (m_commerceProducts.productTable.ContainsKey(commercePurchasedProduct.id))
				{
					m_commerceProducts.productTable.Remove(commercePurchasedProduct.id);
				}
				m_commerceProducts.productTable.Add(commercePurchasedProduct.id, commercePurchasedProduct);
			}
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CommerceUpdateTransport.DoProcess() : 함수에 진입하였습니다");
			m_result = QueryManager.GSCommerce.UpdateTransport(m_charId, m_lostPercent, m_commerceProducts);
			if (m_result != 0)
			{
				WorkSession.WriteStatus("CommerceUpdateTransport.DoProcess() : 수송 정보를 갱신하는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("CommerceUpdateTransport.DoProcess() : 수송 정보를 갱신하는데 실패했습니다.");
			}
			return m_result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CommerceUpdateTransport.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_result);
			return message;
		}
	}
}
