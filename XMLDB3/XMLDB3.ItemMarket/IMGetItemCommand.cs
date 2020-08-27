using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class IMGetItemCommand : ItemMarketCommand
	{
		private const byte packetType = 129;

		public IMGetItemCommand(int serverNo, string accountId, long tradeNo)
		{
			ms = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(ms);
			binaryWriter.Write((byte)129);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(serverNo));
			PacketHelper.WriteStringPacket(binaryWriter, accountId);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(tradeNo));
		}
	}
}
