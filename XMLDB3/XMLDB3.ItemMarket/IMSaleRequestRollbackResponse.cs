using Mabinogi;
using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class IMSaleRequestRollbackResponse : ItemMarketResponse
	{
		public override void Build(BinaryReader _br, Message _message)
		{
			result = IPAddress.NetworkToHostOrder(_br.ReadInt32());
			if (result == 1)
			{
				_message.WriteU8(1);
				return;
			}
			_message.WriteU8(51);
			_message.WriteS32(result);
		}
	}
}
