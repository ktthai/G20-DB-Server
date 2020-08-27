using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class IMItemListCommand : ItemMarketCommand
	{
		private const byte packetType = 49;

		public IMItemListCommand(int ServerNo, string accounId, int pageNo, int pageItemCount, IMSortingType sorting, bool bAsc)
		{
			ms = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(ms);
			binaryWriter.Write((byte)49);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(-1));
			PacketHelper.WriteStringPacket(binaryWriter, accounId);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(pageNo));
			binaryWriter.Write(IPAddress.HostToNetworkOrder(pageItemCount));
			binaryWriter.Write((byte)sorting);
			binaryWriter.Write((byte)(bAsc ? 1 : 0));
		}
	}
}
