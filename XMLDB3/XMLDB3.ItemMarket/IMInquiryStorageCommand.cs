using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class IMInquiryStorageCommand : ItemMarketCommand
	{
		private const byte packetType = 34;

		public IMInquiryStorageCommand(int ServerNo, string accounId, int pageNo, int pageItemCount, IMStorageType storageType)
		{
			ms = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(ms);
			binaryWriter.Write((byte)34);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(ServerNo));
			PacketHelper.WriteStringPacket(binaryWriter, accounId);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(pageNo));
			binaryWriter.Write(IPAddress.HostToNetworkOrder(pageItemCount));
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)storageType));
		}
	}
}
