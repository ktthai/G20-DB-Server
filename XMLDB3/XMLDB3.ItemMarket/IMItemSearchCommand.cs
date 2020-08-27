using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class IMItemSearchCommand : ItemMarketCommand
	{
		private const byte packetType = 50;

		public IMItemSearchCommand(int ServerNo, string accounId, int pageNo, int pageItemCount, string itemName, IMSortingType sorting, bool bAsc, int itemGroup)
		{
			ms = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(ms);
			binaryWriter.Write((byte)50);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(-1));
			PacketHelper.WriteStringPacket(binaryWriter, accounId);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(pageNo));
			binaryWriter.Write(IPAddress.HostToNetworkOrder(pageItemCount));
			PacketHelper.WriteStringPacket(binaryWriter, itemName);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(-1));
			binaryWriter.Write(IPAddress.HostToNetworkOrder(-1));
			binaryWriter.Write(IPAddress.HostToNetworkOrder(itemGroup));
			binaryWriter.Write((byte)sorting);
			binaryWriter.Write((byte)(bAsc ? 1 : 0));
		}
	}
}
