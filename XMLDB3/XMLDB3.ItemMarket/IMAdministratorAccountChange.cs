using System.Collections;
using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class IMAdministratorAccountChange : ItemMarketCommand
	{
		private const byte packetType = 164;

		public IMAdministratorAccountChange(int gameNo, int serverNo, string newNexonId, ArrayList accounts)
		{
			ms = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(ms);
			binaryWriter.Write((byte)164);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(gameNo));
			binaryWriter.Write(IPAddress.HostToNetworkOrder(serverNo));
			PacketHelper.WriteStringPacket(binaryWriter, newNexonId);
			binaryWriter.Write(IPAddress.HostToNetworkOrder(accounts.Count));
			foreach (string account in accounts)
			{
				PacketHelper.WriteStringPacket(binaryWriter, account);
			}
		}
	}
}
