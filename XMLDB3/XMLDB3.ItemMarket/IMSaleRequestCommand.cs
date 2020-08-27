using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class IMSaleRequestCommand : ItemMarketCommand
	{
		private const byte packetType = 65;

		public IMSaleRequestCommand(int ServerNo, string accounId, Item item, string itemName, int price, int itemFee, int itemRegistFee, byte salePeriod)
		{
			ms = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(ms);
			binaryWriter.Write((byte)65);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(ServerNo));
			PacketHelper.WriteStringPacket(binaryWriter, accounId);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(item.Class));
			binaryWriter.Write(0);
			binaryWriter.Write(0);
			PacketHelper.WriteStringPacket(binaryWriter, itemName);
			binaryWriter.Write(IPAddress.HostToNetworkOrder((short)1));
			binaryWriter.Write(IPAddress.HostToNetworkOrder(price));
			binaryWriter.Write(IPAddress.HostToNetworkOrder(itemFee));
			binaryWriter.Write(IPAddress.HostToNetworkOrder(itemRegistFee));
			binaryWriter.Write(salePeriod);
			PacketHelper.WriteItemPacket(binaryWriter, item, ServerNo);
		}
	}
}
