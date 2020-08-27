using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class IMInquiryMyPageCommand : ItemMarketCommand
	{
		private const byte packetType = 36;

		public IMInquiryMyPageCommand(int ServerNo, string accounId, int pageNo, int pageItemCount, bool bSale, IMSortingType sorting, bool bAsc)
		{
			ms = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(ms);
			binaryWriter.Write((byte)36);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(ServerNo));
			PacketHelper.WriteStringPacket(binaryWriter, accounId);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(pageNo));
			binaryWriter.Write(IPAddress.HostToNetworkOrder(pageItemCount));
			binaryWriter.Write((byte)(bSale ? 1 : 0));
			binaryWriter.Write((byte)sorting);
			binaryWriter.Write((byte)(bAsc ? 1 : 0));
		}
	}
}
