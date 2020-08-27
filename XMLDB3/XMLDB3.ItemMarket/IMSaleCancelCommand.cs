using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class IMSaleCancelCommand : ItemMarketCommand
	{
		private const byte packetType = 67;

		public IMSaleCancelCommand(int serverNo, string accountId, long tradeNo)
		{
			ms = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(ms);
			binaryWriter.Write((byte)67);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(serverNo));
			PacketHelper.WriteStringPacket(binaryWriter, accountId);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(tradeNo));
		}
	}
}
