using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class IMGetItemRollbackCommand : ItemMarketCommand
	{
		private const byte packetType = 131;

		public IMGetItemRollbackCommand(int serverNo, string accountId, long tradeNo, int itemLocation)
		{
			ms = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(ms);
			binaryWriter.Write((byte)131);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(serverNo));
			PacketHelper.WriteStringPacket(binaryWriter, accountId);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(tradeNo));
			binaryWriter.Write(IPAddress.HostToNetworkOrder(itemLocation));
		}
	}
}
