using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class IMInquirySaleItemCommand : ItemMarketCommand
	{
		private const byte packetType = 33;

		public IMInquirySaleItemCommand(int ServerNo, string accounId, int pageNo, int pageItemCount)
		{
			ms = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(ms);
			binaryWriter.Write((byte)33);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(ServerNo));
			PacketHelper.WriteStringPacket(binaryWriter, accounId);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(pageNo));
			binaryWriter.Write(IPAddress.HostToNetworkOrder(pageItemCount));
		}
	}
}
