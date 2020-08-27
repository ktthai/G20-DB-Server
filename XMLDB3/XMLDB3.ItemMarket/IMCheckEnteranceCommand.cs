using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class IMCheckEnteranceCommand : ItemMarketCommand
	{
		private const byte packetType = 17;

		public IMCheckEnteranceCommand(int gameNo, int ServerNo, string nexonId, string accounId, string accountName)
		{
			ms = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(ms);
			binaryWriter.Write((byte)17);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(gameNo));
			binaryWriter.Write(IPAddress.HostToNetworkOrder(ServerNo));
			PacketHelper.WriteStringPacket(binaryWriter, nexonId);
			PacketHelper.WriteStringPacket(binaryWriter, accounId);
			PacketHelper.WriteStringPacket(binaryWriter, accountName);
		}
	}
}
