using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class IMGetItemCommitCommand : ItemMarketCommand
	{
		private const byte packetType = 130;

		public IMGetItemCommitCommand(int serverNo, string accountId, long tradeNo)
		{
			ms = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(ms);
			binaryWriter.Write((byte)130);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(serverNo));
			PacketHelper.WriteStringPacket(binaryWriter, accountId);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(tradeNo));
		}
	}
}
