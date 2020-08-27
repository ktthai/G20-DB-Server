using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class IMSaleRequestCommitCommmand : ItemMarketCommand
	{
		private const byte packetType = 66;

		public IMSaleRequestCommitCommmand(int serverNo, string accountId, long tradeNo)
		{
			ms = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(ms);
			binaryWriter.Write((byte)66);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(serverNo));
			PacketHelper.WriteStringPacket(binaryWriter, accountId);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(tradeNo));
		}
	}
}
