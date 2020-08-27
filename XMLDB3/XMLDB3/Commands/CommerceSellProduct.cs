using Mabinogi;
using System.Collections.Generic;

namespace XMLDB3
{
	public class CommerceSellProduct : BasicCommand
	{
		private long m_charId;

		private long m_ducat;

		private int m_postId;

		private int m_postCredit;

		private CommercePurchasedProducts m_commerceProducts;

		private REPLY_RESULT m_result;

		public CommerceSellProduct()
			: base(NETWORKMSG.NET_DB_COMMERCE_T_SELL_PRODUCT)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_commerceProducts = new CommercePurchasedProducts();
			m_commerceProducts.productTable = new Dictionary<int, CommercePurchasedProduct>();
			m_charId = _message.ReadS64();
			m_ducat = _message.ReadS64();
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
			m_postId = _message.ReadS32();
			m_postCredit = _message.ReadS32();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CommerceUpdateProduct.DoProcess() : 함수에 진입하였습니다");
			m_result = QueryManager.GSCommerce.UpdateSellProduct(m_charId, m_ducat, m_commerceProducts, m_postId, m_postCredit);
			if (m_result != 0)
			{
				WorkSession.WriteStatus("CommerceUpdateProduct.DoProcess() : 교역물을 갱신하는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("CommerceUpdateProduct.DoProcess() : 교역물을 갱신하는데 실패했습니다.");
			}
			return m_result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CommerceUpdateProduct.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_result);
			return message;
		}
	}
}
