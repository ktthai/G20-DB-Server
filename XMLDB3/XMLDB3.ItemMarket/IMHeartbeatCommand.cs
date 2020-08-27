using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class IMHeartbeatCommand : ItemMarketCommand
	{
		private const byte packetType = byte.MaxValue;

		public IMHeartbeatCommand(int gameNo, int serverNo)
		{
			ms = new MemoryStream(9);
			BinaryWriter binaryWriter = new BinaryWriter(ms);
			binaryWriter.Write(byte.MaxValue);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(gameNo));
			binaryWriter.Write(IPAddress.HostToNetworkOrder(serverNo));
		}
	}
}
