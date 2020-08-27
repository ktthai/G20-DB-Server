using Mabinogi;
using System.Collections.Generic;

namespace XMLDB3
{
	public class CommerceSystemUpdateALL : BasicCommand
	{
		private string m_strServerName;

		private CommerceSystem m_commerceSystem;

		private REPLY_RESULT m_result;

		public CommerceSystemUpdateALL()
			: base(NETWORKMSG.NET_DB_COMMERCE_E_WRITE_ALL)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_commerceSystem = new CommerceSystem();
			m_commerceSystem.posts = new CommerceSystemPosts();
			m_commerceSystem.products = new CommerceSystemProducts();
			m_commerceSystem.posts.postTable = new Dictionary<int, CommerceSystemPost>();
			m_commerceSystem.products.productTable = new Dictionary<int, CommerceSystemProduct>();
			m_strServerName = _message.ReadString();
			int num = _message.ReadS32();
			for (int i = 0; i < num; i++)
			{
				CommerceSystemPost commerceSystemPost = new CommerceSystemPost();
				commerceSystemPost.id = _message.ReadS32();
				commerceSystemPost.investment = _message.ReadS32();
				commerceSystemPost.commission = _message.ReadS32();
				if (m_commerceSystem.posts.postTable.ContainsKey(commerceSystemPost.id))
				{
					m_commerceSystem.posts.postTable.Remove(commerceSystemPost.id);
				}
				m_commerceSystem.posts.postTable.Add(commerceSystemPost.id, commerceSystemPost);
			}
			int num2 = _message.ReadS32();
			for (int j = 0; j < num2; j++)
			{
				CommerceSystemProduct commerceSystemProduct = new CommerceSystemProduct();
				commerceSystemProduct.stockTable = new Dictionary<int, COStockInfo>();
				commerceSystemProduct.id = _message.ReadS32();
				commerceSystemProduct.price = _message.ReadS32();
				commerceSystemProduct.count = _message.ReadS32();
				int num3 = _message.ReadS32();
				for (int k = 0; k < num3; k++)
				{
					COStockInfo cOStockInfo = new COStockInfo();
					cOStockInfo.idPost = _message.ReadS32();
					cOStockInfo.currentStock = _message.ReadS32();
					cOStockInfo.reserveStock = _message.ReadS32();
					cOStockInfo.price = _message.ReadS32();
					if (commerceSystemProduct.stockTable.ContainsKey(cOStockInfo.idPost))
					{
						commerceSystemProduct.stockTable.Remove(cOStockInfo.idPost);
					}
					commerceSystemProduct.stockTable.Add(cOStockInfo.idPost, cOStockInfo);
				}
				if (m_commerceSystem.products.productTable.ContainsKey(commerceSystemProduct.id))
				{
					m_commerceSystem.products.productTable.Remove(commerceSystemProduct.id);
				}
				m_commerceSystem.products.productTable.Add(commerceSystemProduct.id, commerceSystemProduct);
			}
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CommerceSystemUpdateProduct.DoProcess() : 함수에 진입하였습니다");
			m_result = QueryManager.COCommerce.Update(m_strServerName, m_commerceSystem);
			if (m_result != 0)
			{
				WorkSession.WriteStatus("CommerceSystemUpdateProduct.DoProcess() : 교역물을 갱신하는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("CommerceSystemUpdateProduct.DoProcess() : 교역물을 갱신하는데 실패했습니다.");
			}
			return m_result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CommerceSystemUpdateProduct.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_result);
			return message;
		}
	}
}
