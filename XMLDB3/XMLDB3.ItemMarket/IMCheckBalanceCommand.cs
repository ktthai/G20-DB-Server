using System.IO;

namespace XMLDB3.ItemMarket
{
	public class IMCheckBalanceCommand : ItemMarketCommand
	{
		private const byte packetType = 18;

		public IMCheckBalanceCommand(string strNexonId)
		{
			ms = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(ms);
			binaryWriter.Write((byte)18);
			PacketHelper.WriteStringPacket(binaryWriter, strNexonId);
		}
	}
}
