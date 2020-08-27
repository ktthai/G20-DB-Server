using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class IMSaleRequestRollbackCommand : ItemMarketCommand
	{
		private const byte packetType = 69;

		public IMSaleRequestRollbackCommand(int serverNo, string accountId, long tradeNo)
		{
			ms = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(ms);
			binaryWriter.Write((byte)69);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(serverNo));
			PacketHelper.WriteStringPacket(binaryWriter, accountId);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(tradeNo));
		}
	}
}
