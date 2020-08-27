using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class IMPurchaseCommand : ItemMarketCommand
	{
		private const byte packetType = 68;

		public IMPurchaseCommand(int serverNo, string accountId, string nexonId, long tradeNo)
		{
			ms = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(ms);
			binaryWriter.Write((byte)68);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(serverNo));
			PacketHelper.WriteStringPacket(binaryWriter, accountId);
			PacketHelper.WriteStringPacket(binaryWriter, nexonId);
			binaryWriter.Write(IPAddress.HostToNetworkOrder((short)7));
			binaryWriter.Write((byte)0);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(1));
			binaryWriter.Write(IPAddress.HostToNetworkOrder(tradeNo));
		}
	}
}
