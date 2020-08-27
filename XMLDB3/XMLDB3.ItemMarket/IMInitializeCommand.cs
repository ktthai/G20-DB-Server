using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class IMInitializeCommand : ItemMarketCommand
	{
		private const byte packetType = 1;

		public IMInitializeCommand(int gameNo, int serverNo)
		{
			ms = new MemoryStream(9);
			BinaryWriter binaryWriter = new BinaryWriter(ms);
			binaryWriter.Write((byte)1);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(gameNo));
			binaryWriter.Write(IPAddress.HostToNetworkOrder(serverNo));
		}
	}
}
